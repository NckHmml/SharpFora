using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SharpFora.Security
{
    /// <summary>
    /// Helper class for password related functionality
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// The amount of iterations passed
        /// </summary>
        const int Iterations = 20000;

        /// <summary>
        /// The length of the hash to be generated
        /// </summary>
        /// <remarks>
        /// Default is 160 bits, same as the underlying HMACSHA1
        /// Note: User data model does not use this constant
        /// </remarks>
        const int HashLength = 20;

        /// <summary>
        /// Applies PBKDF2 to <paramref name="password"/>
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="salt">The salt used</param>
        /// <returns>The hashed version of <paramref name="password"/></returns>
        public static byte[] CreatePassword(string password, out byte[] salt)
        {
            var result = new Rfc2898DeriveBytes(password, HashLength, Iterations);
            salt = result.Salt;
            return result.GetBytes(HashLength);
        }

        /// <summary>
        /// Checks if passwords match.
        /// </summary>
        /// <param name="actual">The actual password</param>
        /// <param name="guess">The guess to check against</param>
        /// <param name="salt">The actual salt</param>
        /// <returns>Return true if <paramref name="actual"/> and a <paramref name="guess"/> match</returns>
        public static bool CheckPassword(byte[] actual, string guess, byte[] salt)
        {
            byte[] bGuess = new Rfc2898DeriveBytes(guess, salt, Iterations).GetBytes(HashLength);
            return Compare(actual, bGuess);
        }

        /// <summary>
        /// Compares a <paramref name="password"/> and a <paramref name="guess"/> in length-constant time.
        /// </summary>
        /// <param name="password">The password to compare against</param>
        /// <param name="guess">The value to compare the password against</param>
        /// <returns>Return true if <paramref name="password"/> and a <paramref name="guess"/> match</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static bool Compare(byte[] password, byte[] guess)
        {
            if (password.Length != guess.Length)
                throw new ArgumentOutOfRangeException("Expected both parameters to have the same length");

            int diff = 0;
            for (int i = 0; i < password.Length; i++)
                diff |= password[i] ^ guess[i];

            return diff == 0;
        }
    }
}
