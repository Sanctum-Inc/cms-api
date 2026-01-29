using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Session;
using Application.Users.Commands.Login;
using Application.Users.Commands.Register;
using Application.Users.Queries;
using Domain.Users;
using ErrorOr;
using Infrastructure.Common;
using MapsterMapper;

namespace Infrastructure.Services;
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;
    private readonly ISessionResolver _sessionResolver;

    public UserService(
        IUserRepository userRepository,
        IJwtService jwtService,
        IMapper mapper,
        ISessionResolver sessionResolver)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _mapper = mapper;
        _sessionResolver = sessionResolver;
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
            return Error.Validation("User.NotFound", "User not found.");

        return _mapper.Map<UserResult>(user);
    }

    public async Task<ErrorOr<AuthenticationResult>> Login(string username, string password, CancellationToken cancellationToken)
    {
        // 1. Find user by email
        var user = await _userRepository.GetByEmail(username, cancellationToken);
        if (user == null)
            return Error.Unauthorized("Authentication.Unauthorized", "Username or password is incorrect");

        // 2. Verify password
        var isMatch = PasswordHasher.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        if (!isMatch)
            return Error.Unauthorized("Authentication.Unauthorized", "Username or password is incorrect");

        // 3. Set token expiration
        var expiration = DateTime.UtcNow.AddMinutes(30);

        // 4. Generate access token
        var token = _jwtService.GenerateToken(
            "user",
            user.Email,
            user.Id.ToString(),
            user.Name,
            user.Surname,
            user.FirmId.ToString() ?? ""); // ToDo: Ensure that on register FirmId is set properly

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
            return Error.Validation("User.AlreadyExists", "A user with this email already exists.");
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
            FirmId = new Guid(request.FirmId)
        };

        // 4. Save to DB
        await _userRepository.AddAsync(user, cancellationToken);
        await _userRepository.SaveChangesAsync(cancellationToken);

        return true;
    }
}
