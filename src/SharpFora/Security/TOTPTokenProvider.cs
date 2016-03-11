using Microsoft.AspNet.Identity;
using SharpFora.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SharpFora.Security
{
    public class TOTPTokenProvider : IUserTokenProvider<User>
    {
        public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user) =>
            Task.FromResult(user.TokenSecret.Length > 0);

        public Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user) => 
            Task.FromResult(String.Empty);

        public Task<bool> ValidateAsync(string purpose, string sToken, UserManager<User> manager, User user)
        {
            int token;
            if (!int.TryParse(sToken, out token))
                return Task.FromResult(false);

            bool passed = false;
            long timestamp = TimeStamp();

            // Check 2 in the past, 1 in the future
            for (int i = -2; i <= 1; i++)
            {
                int diff = GenerateToken(user.TokenSecret, timestamp + i);
                diff ^= token;
                if (diff == 0)
                    passed = true;
            }
            return Task.FromResult(passed);
        }

        private static int GenerateToken(byte[] secret, long timestamp)
        {
            byte[] time = BitConverter.GetBytes(timestamp)
                .Reverse()
                .ToArray();

            byte[] hash = new HMACSHA1(secret).ComputeHash(time);
            int offset = hash.Last() & 0x0F;
            int result = (hash[offset] & 0x7F) << 24;
            result |= (hash[offset + 1] & 0xFF) << 16;
            result |= (hash[offset + 2] & 0xFF) << 8;
            result |= (hash[offset + 3] & 0xFF);
            result %= 1000000;

            return result;
        }

        private static long TimeStamp() =>
            (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds / 30);
    }
}
