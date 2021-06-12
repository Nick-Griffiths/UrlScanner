using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace UrlScanner.Server.Infrastructure.Email
{
    internal sealed class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailOptions _options;

        internal EmailService(IOptions<EmailOptions> options, ILogger<EmailService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            if (options == null) throw new ArgumentNullException(nameof(options));
            _options = options.Value;
        }

        public async Task Send(string from, string to, string subject, string content)
        {
            if (!_options.SendingIsEnabled)
            {
                _logger.LogWarning("Email sending is currently disabled."); 
                return;
            }
            
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = content };

            using var client = new SmtpClient();
            await client.ConnectAsync(_options.Smtp.Host, _options.Smtp.Port, _options.Smtp.SecureSocketOptions);
            await client.AuthenticateAsync(_options.Smtp.UserName, _options.Smtp.Password);
            await client.SendAsync(email);
            await client.DisconnectAsync(true);        
        }
    }
}