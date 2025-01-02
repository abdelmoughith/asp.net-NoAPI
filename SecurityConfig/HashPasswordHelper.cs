using System;
using System.Security.Cryptography;
using System.Text;

public static class HashPasswordHelper
{
    
    public static string HashPassword(string password, string salt = null)
    {
        if (password == null)
            throw new ArgumentException("Password is null.", nameof(password));
        if (password.Length == 0)
            throw new ArgumentException("Password lenght 0", nameof(password));
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        // Generate a random salt if not provided
        if (string.IsNullOrEmpty(salt))
        {
            salt = GenerateSalt();
        }

        using (var sha256 = SHA256.Create())
        {
            var combinedPassword = $"{salt}:{password}"; // Concatenate salt and password
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
            var hash = Convert.ToBase64String(hashBytes);

            return $"{salt}:{hash}"; // Store salt and hash together
        }
    }

    
    public static bool VerifyPassword(string password, string storedHashedPassword)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHashedPassword))
            return false;

        var parts = storedHashedPassword.Split(':');
        if (parts.Length != 2)
            throw new FormatException("The stored hashed password format is invalid.");

        var salt = parts[0];
        var storedHash = parts[1];

        var hashedPassword = HashPassword(password, salt);

        return hashedPassword == storedHashedPassword;
    }

    
    private static string GenerateSalt()
    {
        var rng = new RNGCryptoServiceProvider();
        var saltBytes = new byte[16]; // 128-bit salt
        rng.GetBytes(saltBytes);

        return Convert.ToBase64String(saltBytes);
    }
}
