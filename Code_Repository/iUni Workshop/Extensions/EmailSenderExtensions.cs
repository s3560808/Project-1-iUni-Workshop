using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using iUni_Workshop.Services;

namespace iUni_Workshop.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "iUni Workshop - Confirm your email",
                $"Hi, user {email.ToString()}:" +
                $"</br>" +
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>" +
                $"</br>" +
                $"If you cannot click above link, please use following url to confirm:" +
                $"</br>" +
                $"{HtmlEncoder.Default.Encode(link)}"+
                $"</br>"+
                $"iUni Workshop Team" +
                $"</br>"+
                $"Kind Regards"
            );
        }
    }
}
