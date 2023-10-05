using System.Net.Mail;
using System.Net;

namespace SumincogarBackend.Services.EnviarEmails
{
    public class EnviarEmail : IEnviarEmail
    {
        private readonly IConfiguration _configuration;

        public EnviarEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool SendEmail(string recipientEmail, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_configuration["SMTP:email"], _configuration["SMTP:password"]),
                EnableSsl = true,
            };

            // Handle SSL/TLS certificate validation
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => true;

            MailMessage mail = new MailMessage(_configuration["SMTP:email"], recipientEmail, subject, body);
            mail.IsBodyHtml = true;

            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
