using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace test.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //var emailMessage = new MimeMessage();
            //emailMessage.From.Add(MailboxAddress.Parse("hema@gmail.com"));
            //emailMessage.To.Add(MailboxAddress.Parse(email));
            //emailMessage.Subject = subject;
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            //{
            //    Text = htmlMessage
            //};

            //using( var client=new SmtpClient())
            //{
            //    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            //    client.Authenticate("WebTest24790@gmail.com", "Hema123!");
            //    client.Send(emailMessage);
            //    client.Disconnect(true);
            //}


            return Task.CompletedTask;
        }
    }
}
