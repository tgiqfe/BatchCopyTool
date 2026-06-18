using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BatchCopyTool.Lib
{
    internal class EncryptionUtil
    {
        const string DEF_AESKEY = "1234567890abcdef";
        const string DEF_AESIV = "abcdef1234567890";

        #region AES Key set

        const string KEYSET_FILE = "enc.txt";

        private static (string, string) GenerateKeySet()
        {
            string keysetFile = Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                KEYSET_FILE);
            if (File.Exists(keysetFile))
            {
                var content = File.ReadAllText(keysetFile);
                var key = "";
                var iv = "";
                int count = 0;
                foreach (var line in Regex.Split(content, @"\r?\n"))
                {
                    if (!Regex.Match(line, @"^\s*[#;]").Success)
                    {
                        if (count == 0)
                        {
                            key = line.Trim();
                        }
                        else if (count == 1)
                        {
                            iv = line.Trim();
                            break;
                        }
                        count++;
                    }
                }
                return (key, iv);
            }
            return (DEF_AESKEY, DEF_AESIV);
        }

        #endregion

        public static string AesEncrypt(string text_plain)
        {
            string result;
            using (Aes aes = Aes.Create())
            {
                var (key, iv) = GenerateKeySet();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text_plain);
                    }
                    result = Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
            return result;
        }

        public static string AesDecrypt(string text_cipher)
        {
            string result;
            using (Aes aes = Aes.Create())
            {
                var (key, iv) = GenerateKeySet();
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(text_cipher)))
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                {
                    try
                    {
                        result = srDecrypt.ReadToEnd();
                    }
                    catch
                    {
                        result = null;
                    }
                }
            }
            return result;
        }
    }
}
