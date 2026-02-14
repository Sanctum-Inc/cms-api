using System.Net;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Email.Commands.Add;
using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Queries;
using Domain.Emails;
using Domain.Users;
using ErrorOr;
using Infrastructure.Common;
using Infrastructure.Config;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly ISessionResolver _sessionResolver;
    private readonly IUserRepository _userRepository;
    private readonly IEmailRepository _emailRepository;
    private readonly EnvironmentOptions  _environmentOptions;
    private readonly IFirmRepository  _firmRepository;


    public UserService(
        IUserRepository userRepository,
        IJwtService jwtService,
        IMapper mapper,
        ISessionResolver sessionResolver,
        IEmailRepository emailRepository,
        IOptions<EnvironmentOptions> environmentOptions,
        IFirmRepository  firmRepository)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mapper = mapper;
        _sessionResolver = sessionResolver;
        _emailRepository = emailRepository;
        _environmentOptions = environmentOptions.Value;
        _firmRepository = firmRepository;
    }

    public async Task<ErrorOr<UserResult>> GetUserById(string id, CancellationToken cancellationToken)
    {
        User? user = null;
        if (id is "current")
        {
            user = await _userRepository.GetByIdAsync(Guid.Parse(_sessionResolver.UserId!), cancellationToken);
        }
        else
        {
            user = await _userRepository.GetByIdAsync(Guid.Parse(id), cancellationToken);
        }

        if (user == null)
        {
            return Error.Validation("User.NotFound", "User not found.");
        }

        return _mapper.Map<UserResult>(user);
    }

    public async Task<ErrorOr<AuthenticationResult>> Login(string username, string password,
        CancellationToken cancellationToken)
    {
        // 1. Find user by email
        var user = await _userRepository.GetByEmail(username, cancellationToken);
        if (user == null)
        {
            return Error.Unauthorized("Authentication.Unauthorized", "Username or password is incorrect");
        }

        // 2. Verify password
        var isMatch = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        if (!isMatch)
        {
            return Error.Unauthorized("Authentication.Unauthorized", "Username or password is incorrect");
        }

        // 3. Set token expiration
        var expiration = DateTime.UtcNow.AddMinutes(30);

        // 4. Generate access token
        var token = _jwtService.GenerateToken(
            "user",
            user.Email,
            user.Id.ToString(),
            user.Name,
            user.Surname,
            user.FirmId.ToString() ?? "",
            1,
            false); // ToDo: Ensure that on register FirmId is set properly

        // 5. Generate refresh token (simple example — use a secure random generator in production)
        // ToDo: Refresh token logic to be implemented
        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

        // (Optional) Store refresh token in DB for the user
        // await _refreshTokenRepository.Save(user.Id, refreshToken, expiration.AddDays(7));

        // 6. Return authentication result
        return new AuthenticationResult(
            expiration,
            token,
            refreshToken
        );
    }

    public async Task<ErrorOr<bool>> Register(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Check if user already exists
        var existingUser = await _userRepository.GetByEmail(request.Email, cancellationToken);
        if (existingUser != null)
        {
            return Error.Validation("Email", "A user with this email already exists.");
        }

        var firm = await _firmRepository.GetByIdAsync(request.FirmId, cancellationToken);
        if (firm == null)
        {
            return Error.NotFound("FirmId", "Firm not found.");
        }

        // 2. Hash the password
        var (hash, salt) = PasswordHasher.HashPassword(request.Password);

        // 3. Create user
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname,
            MobileNumber = request.MobileNumber,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = UserRole.FirmUser,
            FirmId = request.FirmId,
            IsEmailVerified =  false,
        };

        // 4. Save to DB
        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        var emailResult = await SendConfirmEmailOtp(user, cancellationToken);
        if (emailResult.IsError)
        {
            return Error.Unexpected("User.ConfirmEmail", "There was an error confirming your email. Please try again in settings.");
        }

        return true;
    }

    public async Task<ErrorOr<bool>> ResendConfirmEmailOtp(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmail(email, cancellationToken);

        if (user == null)
            return Error.NotFound();

        if (user.IsEmailVerified)
            return Error.Conflict("Email already verified.");

        if (user.LastVerificationEmailSentAt.HasValue &&
            user.LastVerificationEmailSentAt.Value.AddMinutes(2) > DateTime.UtcNow)
        {
            return Error.Validation("Please wait before requesting another email.");
        }

        // Invalidate old tokens
        user.EmailVerificationTokenVersion++;

        user.LastVerificationEmailSentAt = DateTime.UtcNow;
        user.VerificationEmailSentCount++;

        await _userRepository.SaveChangesAsync(cancellationToken);

        return await SendConfirmEmailOtp(user, cancellationToken);
    }

    private async Task<ErrorOr<bool>> SendConfirmEmailOtp(User user, CancellationToken cancellationToken)
    {
        var token = _jwtService.GenerateToken(
            user.Role.ToString(),
            user.Email,
            user.Id.ToString(),
            user.Name,
            user.Surname,
            user.FirmId.ToString(),
            1,
            true,
            "1440");

        var email = new Email()
        {
            Status = EmailStatus.Pending,
            ToAddresses = user.Email,
            Subject = "Lexcase OTP verification",
            Body = GenerateOtpEmailContent(token, user.Email),
            RetryCount = 0,
            IsHtml = true,
            UserId = user.Id,
            CreatedBy =  user.Id,
            User = user,
        };

        await _emailRepository.AddAsync(email, cancellationToken);
        await _emailRepository.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<ErrorOr<string>> ConfirmEmailOtp(string token, string email, CancellationToken cancellationToken)
    {
        var tokenValidationResult = await _jwtService.VerifyToken(token);
        if (!tokenValidationResult.IsValid || !tokenValidationResult.Claims.ContainsKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"))
        {
            return _environmentOptions.FrontendUrl + "email-verified?status=error&email="  + email;
        }

        var tokenEmail = tokenValidationResult.Claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"].ToString();

        if (string.IsNullOrEmpty(tokenEmail))
        {
            return _environmentOptions.FrontendUrl + "email-verified?status=error&email="  + email;
        }

        var user = await _userRepository.GetByEmail(tokenEmail, cancellationToken);
        if (user == null)
        {
            return _environmentOptions.FrontendUrl + "email-verified?status=error&email="  + email;
        }

        if (user.IsEmailVerified)
        {
            return _environmentOptions.FrontendUrl + "email-verified?status=verified";
        }

        if (user.EmailVerificationTokenVersion.ToString() != tokenValidationResult.Claims["version"].ToString())
        {
            return _environmentOptions.FrontendUrl + "email-verified?status=error&email="  + email;
        }

        user.IsEmailVerified = true;
        await _userRepository.SaveChangesAsync(cancellationToken);

        return _environmentOptions.FrontendUrl + "email-verified?status=success";
    }

    private string GenerateOtpEmailContent(string token, string email)
    {
        var encodedToken = WebUtility.UrlEncode(token);
        var encodedEmail = WebUtility.UrlEncode(email);

        var verificationUrl =
            $"{_environmentOptions.BackendUrl}api/User/confirm-email?token={encodedToken}&email={encodedEmail}";

        var html = """
                   <!DOCTYPE html>
                   <html lang="en">
                   <head>
                       <meta charset="UTF-8">
                       <meta name="viewport" content="width=device-width, initial-scale=1.0">
                       <meta http-equiv="X-UA-Compatible" content="IE=edge">
                       <title>Verify Your Email</title>
                       <style>
                           /* Reset styles for email clients */
                           body, table, td, a { -webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%; }
                           table, td { mso-table-lspace: 0pt; mso-table-rspace: 0pt; }
                           img { -ms-interpolation-mode: bicubic; border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none; }
                           table { border-collapse: collapse !important; }
                           body { height: 100% !important; margin: 0 !important; padding: 0 !important; width: 100% !important; background-color: #f1f5f9; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; }

                           /* Mobile responsive styles */
                           @media screen and (max-width: 600px) {
                               .container { width: 100% !important; }
                               .content-padding { padding: 20px !important; }
                           }
                       </style>
                   </head>
                   <body>
                       <table border="0" cellpadding="0" cellspacing="0" width="100%">
                           <tr>
                               <td align="center" style="padding: 40px 0;">
                                   <!-- Main Container -->
                                   <table border="0" cellpadding="0" cellspacing="0" width="600" class="container" style="background-color: #ffffff; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.1);">

                                       <!-- Header/Logo Section -->
                                       <tr>
                                           <td align="center" style="padding: 40px 40px 20px 40px; background-color: #0f172a;">
                                               <!-- Fallback to text if image fails -->
                                               <h1 style="color: #ffffff; margin: 0; font-size: 24px; font-weight: 800; letter-spacing: -1px; text-transform: uppercase;">
                                                   LexCase <span style="font-weight: 300; color: #94a3b8;">CMS</span>
                                               </h1>
                                           </td>
                                       </tr>

                                       <!-- Body Content -->
                                       <tr>
                                           <td class="content-padding" style="padding: 40px; color: #1e293b;">
                                               <h2 style="margin: 0 0 20px 0; font-size: 22px; font-weight: 700; line-height: 1.2;">Verify your email address</h2>
                                               <p style="margin: 0 0 24px 0; font-size: 16px; line-height: 1.6; color: #475569;">
                                                   Hello there,<br><br>
                                                   Thank you for joining LexCase CMS. To complete your account setup and ensure the security of your legal data, please verify your email address by clicking the button below.
                                               </p>

                                               <!-- CTA Button -->
                                               <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                   <tr>
                                                       <td align="center" style="padding: 10px 0 30px 0;">
                                                           <a href="__VERIFY_URL__" target="_blank" style="background-color: #1b3b5f; border-radius: 8px; color: #ffffff; display: inline-block; font-size: 16px; font-weight: 700; line-height: 50px; text-align: center; text-decoration: none; width: 240px; -webkit-text-size-adjust: none;">Verify Email Address</a>
                                                       </td>
                                                   </tr>
                                               </table>

                                               <p style="margin: 0 0 10px 0; font-size: 14px; line-height: 1.6; color: #64748b;">
                                                   If the button above doesn't work, copy and paste this link into your browser:
                                               </p>
                                               <p style="margin: 0 0 24px 0; font-size: 13px; line-height: 1.6; color: #3b82f6; word-break: break-all;">
                                                   __VERIFY_URL__
                                               </p>

                                               <hr style="border: 0; border-top: 1px solid #e2e8f0; margin: 30px 0;">

                                               <p style="margin: 0; font-size: 13px; line-height: 1.6; color: #94a3b8;">
                                                   <strong>Why did I receive this?</strong><br>
                                                   You're receiving this because a LexCase CMS account was created using this email address. If you didn't create an account, you can safely ignore this email.
                                               </p>
                                           </td>
                                       </tr>

                                       <!-- Footer -->
                                       <tr>
                                           <td style="padding: 0 40px 40px 40px; background-color: #ffffff;">
                                               <table border="0" cellpadding="0" cellspacing="0" width="100%" style="border-top: 1px solid #f1f5f9; padding-top: 20px;">
                                                   <tr>
                                                       <td align="center" style="color: #94a3b8; font-size: 12px; line-height: 1.4;">
                                                           &copy; 2026 LexCase Legal Solutions. All rights reserved.<br>
                                                           123 Legal Square, Sandton, Johannesburg, 2196
                                                       </td>
                                                   </tr>
                                               </table>
                                           </td>
                                       </tr>
                                   </table>
                               </td>
                           </tr>
                       </table>
                   </body>
                   </html>
                   """;

        return html.Replace("__VERIFY_URL__", verificationUrl);
    }
}
