
using IdentityMail.Web.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace IdentityMail.Web.Services.EmailServices
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;   
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message=new MimeMessage();
            //Gönderen
            message.From.Add(new MailboxAddress(_emailSettings.SenderName,_emailSettings.SenderEmail));
            //Alıcı
            message.To.Add(MailboxAddress.Parse(email));
            //Konu
            message.Subject=subject;
            //İçerik
            var bodyBuilder=new BodyBuilder
            {
                HtmlBody=htmlMessage
            };
            message.Body=bodyBuilder.ToMessageBody();
            using var client=new SmtpClient();
            try
            {
                //connect and authenticate
                await client.ConnectAsync(_emailSettings.SmtpServer,_emailSettings.Port,SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Username,_emailSettings.Password);

                //mail gönder
                await client.SendAsync(message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
