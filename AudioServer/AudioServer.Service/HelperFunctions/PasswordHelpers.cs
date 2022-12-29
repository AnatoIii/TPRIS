using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace AudioServer.Service.HelperFunctions
{
    public class PasswordHelpers
    {
        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            return salt;
        }

        /// <summary>
        /// Hashes the password with the given salt.
        /// </summary>
        public static string HashPassword(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

        /// <summary>
        /// Validates that the given password and salt are hashed to the specified hash.
        /// </summary>
        public static bool ValidatePassword(string password, string salt, string hash)
            => HashPassword(password, Convert.FromBase64String(salt)) == hash;
    }
}