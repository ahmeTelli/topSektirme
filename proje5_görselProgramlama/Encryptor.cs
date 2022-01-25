using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace topSektirme
{
    public static class Encryptor
    {
        private static string password = "password";
        private static readonly byte[] salt =
            Encoding.Unicode.GetBytes("ORANGE06");
        private static readonly int iterations = 2000;

        public static string Encrypt(string backupdata)
        {
            byte[] encryptedData;
            byte[] plainBytes = Encoding.Unicode.GetBytes(backupdata);

            var aes = Aes.Create();
            var pbdfk2 = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = pbdfk2.GetBytes(32);
            aes.IV = pbdfk2.GetBytes(16);
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(
                    ms,aes.CreateEncryptor(),
                    CryptoStreamMode.Write))
                {
                    cs.Write(plainBytes, 0, plainBytes.Length);
                }
                encryptedData = ms.ToArray();
            }
            return Convert.ToBase64String(encryptedData);
        }

        public static string Decrypt(string cryptoText)
        {
            byte[] plainBytes;
            byte[] cryptoBytes = Convert.FromBase64String(cryptoText);

            var aes = Aes.Create();
            var pbdk2 = new Rfc2898DeriveBytes(password, salt, iterations);
            aes.Key = pbdk2.GetBytes(32);
            aes.IV = pbdk2.GetBytes(16);

            using(var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(
                    ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cryptoBytes, 0, cryptoBytes.Length);
                }
                plainBytes = ms.ToArray();
            }
            return Encoding.Unicode.GetString(plainBytes);
        }
    }
}
