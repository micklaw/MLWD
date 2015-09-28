using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Website.Domain.Shared.Services
{
    public class EncryptionService : WebsiteService
    {
        private byte[] GetHash(string inputString)
        {
            var algorithm = MD5.Create(); 

            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string GetHashString(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
            {
                return null;
            }

            var builder = new StringBuilder();

            foreach (byte b in GetHash(inputString))
            {
                builder.Append(b.ToString("X2"));
            }

            return builder.ToString();
        }
    }
}
