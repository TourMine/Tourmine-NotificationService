using System.Net.Mail;
using System.Net;

namespace Tourmine.NotificationService.Services
{
    public static class EmailService
    {
        private static readonly string _smtpHost = "smtp.gmail.com"; 
        private static readonly int _smtpPort = 587;
        private static readonly string _emailFrom = $"{Settings.EmailFrom}";
        private static readonly string _emailPassword = $"{Settings.Password}";

        public static async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (var client = new SmtpClient(_smtpHost, _smtpPort))
                {
                    Console.WriteLine("Sending email...");

                    // Não ativar SSL diretamente, usar STARTTLS
                    client.EnableSsl = true;  // Habilitar SSL para a porta 587
                    client.Credentials = new NetworkCredential(_emailFrom, _emailPassword); // Suas credenciais

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailFrom),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true, // Definir o corpo do e-mail como HTML
                    };

                    mailMessage.To.Add(toEmail); // Adicionar o destinatário

                    // Enviar o e-mail de forma assíncrona
                    await client.SendMailAsync(mailMessage);

                    Console.WriteLine($"Email sent to {toEmail}");
                }
            }
            catch (SmtpException smtpEx)
            {
                Console.WriteLine($"SMTP Exception: {smtpEx.Message}");
                Console.WriteLine($"Stack Trace: {smtpEx.StackTrace}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
