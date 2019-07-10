using System;
using System.Security.Cryptography;
using VerticalSliceArchitecture.Core.Services;

namespace VerticalSliceArchitecture.Infrastructure.Services
{
    public class Hasher : IHasher
    {
        private static readonly int DeriveBytesIterationsCount = 10000;
        private static readonly int SaltSize = 40;

        public string GetSalt()
        {
            using (var random = new RNGCryptoServiceProvider())
            {
                var saltBytes = new byte[SaltSize];
                random.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        public string GetHash(string value, string salt)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Can not generate hash from an empty value.", nameof(value));
            }

            if (string.IsNullOrWhiteSpace(salt))
            {
                throw new ArgumentException("Can not use an empty salt from hashing value.", nameof(value));
            }

            var pbkdf2 = new Rfc2898DeriveBytes(value, GetBytes(salt), DeriveBytesIterationsCount);

            return Convert.ToBase64String(pbkdf2.GetBytes(SaltSize));
        }

        public (string hash, string salt) GetHash(string value)
        {
            var salt = GetSalt();
            var hash = GetHash(value, salt);
            return (hash, salt);
        }

        private static byte[] GetBytes(string value)
        {
            var bytes = new byte[value.Length * sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);

            return bytes;
        }
    }
}