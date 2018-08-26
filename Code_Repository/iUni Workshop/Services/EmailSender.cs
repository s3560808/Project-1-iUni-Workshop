using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace iUni_Workshop.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            MailMessage msg = new MailMessage();

            var smtp = new SmtpClient
            {
                Host = "smtp.exmail.qq.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = true,
                Credentials = new System.Net.NetworkCredential("no-reply@iuni.ml", "1234qwAS./")
            };

            MailMessage mailObj = new MailMessage("no-reply@iuni.ml", email, subject, message);
            mailObj.IsBodyHtml = true;
            mailObj.Priority = MailPriority.High;

            try
            {
                smtp.Send(mailObj);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                msg.Dispose();
            }
            return Task.CompletedTask;
        }
    }
}
