using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace bank_app.utility
{
    public static class PasswordHasher
    {
        private const int _SaltSize = 32;
        private const int _HashSize = 32;
        private const int _Iterations = 200000;

        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(_SaltSize);

            using var keyDeriver = new Rfc2898DeriveBytes(password, salt, _Iterations, HashAlgorithmName.SHA256);

            byte[] hash = keyDeriver.GetBytes(_HashSize);

            string base64Salt = Convert.ToBase64String(salt);
            string base64Hash = Convert.ToBase64String(hash);

            return $"{_Iterations}.{base64Salt}.{base64Hash}";
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            string[]? parts = storedHash.Split(".");

            if (parts.Length != 3)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out int iterations))
            {
                return false;
            }

            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] expectedHash = Convert.FromBase64String(parts[2]);

            using var keyDeriver = new Rfc2898DeriveBytes(password, salt, _Iterations, HashAlgorithmName.SHA256);

            byte[] actualHash = keyDeriver.GetBytes(expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
