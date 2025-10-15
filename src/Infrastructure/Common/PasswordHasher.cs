using System;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Common;
public static class PasswordHasher
{
    private const int _saltSize = 16; // 128 bit
    private const int _keySize = 32;  // 256 bit
    private const int _iterations = 10000;

    public static (string hash, string salt) HashPassword(string password)
    {
        // 1. Generate a random salt
        using var rng = RandomNumberGenerator.Create();
        var saltBytes = new byte[_saltSize];
        rng.GetBytes(saltBytes);

        // 2. Derive key from password and salt
        using var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, _iterations, HashAlgorithmName.SHA256);
        var keyBytes = pbkdf2.GetBytes(_keySize);

        // 3. Convert to Base64 for storage
        var salt = Convert.ToBase64String(saltBytes);
        var hash = Convert.ToBase64String(keyBytes);

        return (hash, salt);
    }

    public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
    {
        var saltBytes = Convert.FromBase64String(storedSalt);
        var storedHashBytes = Convert.FromBase64String(storedHash);

        // Hash entered password with stored salt
        using var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, saltBytes, _iterations, HashAlgorithmName.SHA256);
        var enteredHashBytes = pbkdf2.GetBytes(_keySize);

        // Compare byte arrays securely
        return CryptographicOperations.FixedTimeEquals(storedHashBytes, enteredHashBytes);
    }
}
