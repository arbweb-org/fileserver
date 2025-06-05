using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace drive
{
    public static class Helper
    {
        class SmtpOptions
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
        }

        public static string Admin { get; set; }
        public static string Organization { get; set; }

        static string Salt { get; set; }
        static byte[] _key;
        static string Key
        {
            get
            {
                return ToHex(_key);
            }
            set
            {
                _key = FromHex(value);
            }
        }
        static SmtpOptions Smtp { get; set; }

        public static void Initialize(ConfigurationManager configuration)
        {
            Admin = configuration.GetSection("Admin").Value ?? string.Empty;
            Organization = configuration.GetSection("Organization").Value ?? string.Empty;
            Salt = configuration.GetSection("Salt").Value ?? string.Empty;
            Key = configuration.GetSection("Key").Value ?? string.Empty;
            Smtp = configuration.GetSection("Smtp").Get<SmtpOptions>() ?? new SmtpOptions();
        }

        public static string ToHex(byte[] bytes)
        {
            return Convert.ToHexString(bytes).ToLower();
        }

        public static byte[] FromHex(string hex)
        {
            return Convert.FromHexString(hex);
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
                tripleDES.Key = _key;
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform transform = tripleDES.CreateEncryptor();
                byte[] result = transform.TransformFinalBlock(data, 0, data.Length);
                return ToHex(result) + "-" + Hash(message);
            }
        }

        public static string Decrypt(string cipher)
        {
            if (string.IsNullOrEmpty(cipher))
            {
                return null;
            }

            string[] parts = cipher.Split("-");
            if (parts.Length != 2)
            {
                return null;
            }

            byte[] data = Convert.FromHexString(parts[0]);
            using (TripleDES tripleDES = TripleDES.Create())
            {
                tripleDES.Key = _key;
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
            SmtpClient smtpClient = new SmtpClient(Smtp.Server)
            {
                Port = Smtp.Port,
                Credentials = new NetworkCredential(Smtp.User, Smtp.Password),
                EnableSsl = true,
            };

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(Smtp.User),
                Subject = subject,
                Body = message,
                IsBodyHtml = false,
                To = { email }
            };

            smtpClient.Send(mailMessage);
        }

        // Validate token and return user id, 0 if admin, -1 if invalid or expired
        public static long ParseToken(string token)
        {
            string message = Decrypt(token);

            if (message == null)
            {
                return -1;
            }

            var parts = message.Split("-");
            if (parts.Length != 2)
            {
                return -1;
            }

            long time = long.Parse(parts[0]);
            var old = DateTime.FromFileTimeUtc(time);
            var now = DateTime.Now;
            if (now.ToFileTimeUtc() > old.AddHours(1).ToFileTimeUtc())
            {
                return -1;
            }

            long id;
            if (!long.TryParse(parts[1], out id))
            {
                return -1;
            }

            return id;
        }
    }
}