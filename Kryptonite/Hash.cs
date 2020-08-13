using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Kryptonite
{
    public class Hash
    {
        public static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var rndNumber = new byte[16];
                rng.GetBytes(rndNumber);

                return rndNumber;
            }
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);

            return ret;
        }

        public static byte[] HashPasswordWithSalt(byte[] password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Combine(password, salt));
            }
        }
    }
}
