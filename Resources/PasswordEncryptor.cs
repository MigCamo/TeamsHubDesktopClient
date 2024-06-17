using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Security.Cryptography;
using System.Text;

namespace TeamsHubDesktopClient.Resources
{
    public class PasswordEncryptor
    {
        private string EncryptPassword(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            string passwordMD5 = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();

            return passwordMD5;
        }

        public string GetEncryptedPassword(string password)
        {
            return EncryptPassword(password);
        }
    }
}
