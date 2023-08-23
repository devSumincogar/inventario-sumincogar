namespace SumincogarBackend.Services.EnviarEmails
{
    public interface IEnviarEmail
    {
        bool SendEmail(string recipientEmail, string subject, string body);
    }
}
