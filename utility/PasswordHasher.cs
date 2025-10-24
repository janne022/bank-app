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
        // 256 bit salt (needed to make different keys) and hash (length of encrypted password)
        // 200k iterations to make it slower and more costly for attackers to try and brute force.
        private const int _SaltSize = 32;
        private const int _HashSize = 32;
        private const int _Iterations = 200000;

        /// <summary>
        /// Generates a hashed representation of the specified password using PBKDF2 with SHA-256.
        /// </summary>
        /// <remarks>Uses a cryptographic random number generator to create a salt and applies
        /// the PBKDF2 algorithm with SHA-256 to derive a secure hash.</remarks>
        /// <returns>A string containing the iteration count, salt, and hash, each encoded in Base64 and separated by periods.</returns>
        public static string Hash(string password)
        {
            // generates and stores 32 random bytes.
            byte[] salt = RandomNumberGenerator.GetBytes(_SaltSize);

            // Creates secure key/password with SHA-256 alghorithm.
            using var mixedKey = new Rfc2898DeriveBytes(password, salt, _Iterations, HashAlgorithmName.SHA256);

            // stores that secure key we just created.
            byte[] hash = mixedKey.GetBytes(_HashSize);

            // turns binary data into strings.
            string base64Salt = Convert.ToBase64String(salt);
            string base64Hash = Convert.ToBase64String(hash);

            // return as a string.
            return $"{_Iterations}.{base64Salt}.{base64Hash}";
        }

        /// <summary>
        /// Verifies whether the specified password matches the stored hash.
        /// </summary>
        /// <remarks>The stored hash must be in a specific format consisting of iterations, salt, and
        /// hash, separated by periods. If the format is incorrect or parsing fails, the method returns <see
        /// langword="false"/>.</remarks>
        /// <param name="password">The password to verify.</param>
        /// <param name="storedHash">The stored hash to compare against, expected to be in the format "iterations.salt.hash".</param>
        /// <returns><see langword="true"/> if the password matches the stored hash; otherwise, <see langword="false"/>.</returns>
        public static bool VerifyPassword(string password, string storedHash)
        {
            // split the string into an array of strings consisting of [iteration] [salt] [hash]
            string[]? parts = storedHash.Split(".");

            // If the array isn't three parts, return false (this means the stored hash is incorrectly formatted)
            if (parts.Length != 3)
            {
                return false;
            }

            // parses out the number of iterations from array.
            if (!int.TryParse(parts[0], out int iterations))
            {
                return false;
            }

            // converts the string version of salt and hash into byte arrays for later comparison.
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] expectedHash = Convert.FromBase64String(parts[2]);

            // creates a mixed key as in the Hash() method, with inputed password.
            using var mixedKey = new Rfc2898DeriveBytes(password, salt, _Iterations, HashAlgorithmName.SHA256);

            // converts and stores the hash from the newly made mixed key.
            byte[] actualHash = mixedKey.GetBytes(expectedHash.Length);

            // Returns true if the keys match. Returns false if not.
            return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
        }
    }
}
