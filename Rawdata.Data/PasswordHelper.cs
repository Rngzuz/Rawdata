using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Rawdata.Data
{
    public class PasswordHelper
    {
        public static string CreateHash(string value, string salt, int iterations = 10000, int size = 32)
        {
            var bytes = KeyDerivation.Pbkdf2(
                password: value,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: iterations,
                numBytesRequested: size
            );

            return Convert.ToBase64String(bytes);
        }

        public static string CreateSalt(int size = 16)
        {
            byte[] bytes = new byte[size];

            using (var generator = RandomNumberGenerator.Create()) {
                generator.GetBytes(bytes);
                return Convert.ToBase64String(bytes);
            }
        }

        public static string CreatePassword(string value)
        {
            var salt = CreateSalt();

            return $"{CreateHash(value, salt)}:{salt}";
        }

        public static bool Validate(string value, string hash)
        {
            var password = hash.Split(':');

            return CreateHash(value, password[1]) == password[0];
        }
    }
}
