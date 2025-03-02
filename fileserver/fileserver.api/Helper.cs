using System.Net.Mail;
using System.Net;
using System.Text;
using System.Security.Cryptography;

namespace fileserver.api
{
    public static class Helper
    {
        public static string Password { get; set; }
        public static string Salt { get; set; }

        public static string MailServer { get; set; }
        public static int MailPort { get; set; }
        public static string MailUser { get; set; }
        public static string MailPassword { get; set; }

        public static string ToHex(byte[] hash)
        {
            return Convert.ToHexString(hash).ToLower();
        }

        public static string Hash(string message)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] data = Encoding.UTF8.GetBytes(Salt + "-" + message);

                byte[] hash = sha256Hash.ComputeHash(data);
                return ToHex(hash.Take(4).ToArray());
            }
        }

        public static string Encrypt(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            using (TripleDES tripleDES = TripleDES.Create())
            {
                tripleDES.Key = Encoding.UTF8.GetBytes(Salt);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = tripleDES.CreateEncryptor();
                byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                return ToHex(result) + "-" + Hash(message);
            }
        }

        public static string Decrypt(string cipher)
        {
            string[] parts = cipher.Split("-");
            if (parts.Length != 2)
            {
                return null;
            }

            byte[] data = Convert.FromHexString(parts[0]);
            using (TripleDES tripleDES = TripleDES.Create())
            {
                tripleDES.Key = Encoding.UTF8.GetBytes(Salt);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = tripleDES.CreateDecryptor();
                byte[] result = transform.TransformFinalBlock(data, 0, data.Length);

                string message = Encoding.UTF8.GetString(result);
                if (Hash(message) != parts[1])
                {
                    return null;
                }

                return message;
            }
        }

        public static void Email(string email, string subject, string message)
        {
            return;

            SmtpClient smtpClient = new SmtpClient(MailServer)
            {
                Port = MailPort,
                Credentials = new NetworkCredential(MailUser, MailPassword),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(MailUser),
                Subject = subject,
                Body = message,
                IsBodyHtml = false,
                To = { email }
            };

            smtpClient.Send(mailMessage);
        }

        public static Boolean ValidateToken(string token)
        {
            if (token != null)
            {
                string[] parts = token.Split("-");
                if (parts.Length == 2)
                {
                    long time = long.Parse(parts[0]);
                    if (
                        DateTime.FromFileTimeUtc(time).AddHours(1).ToFileTimeUtc() > DateTime.Now.ToFileTimeUtc() &&
                        parts[1] == Helper.Hash(parts[0]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}