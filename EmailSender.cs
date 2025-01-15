using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace parser_OZON
{
    using System;
    using System.Net;
    using System.Net.Mail;

    internal class EmailSender
    {
        private string smtpServer = "smtp.example.com"; // Замените на ваш SMTP-сервер
        private int smtpPort = 587; // Порт (обычно 587 для TLS)
        private string smtpUser = "your_email@example.com"; // Ваш email
        private string smtpPass = "your_password"; // Ваш пароль

        public void SendEmail(string recipientEmail, string subject, string message)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(smtpUser);
                    mail.To.Add(recipientEmail);
                    mail.Subject = subject;
                    mail.Body = message;

                    using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                    {
                        smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                        smtpClient.EnableSsl = true; // Включаем SSL
                        smtpClient.Send(mail);
                    }
                }

                Console.WriteLine("Письмо отправлено успешно!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке письма: {ex.Message}");
            }
        }
    }

}
