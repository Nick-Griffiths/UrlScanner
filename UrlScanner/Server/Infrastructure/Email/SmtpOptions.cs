using System.Diagnostics.CodeAnalysis;
using MailKit.Security;

namespace UrlScanner.Server.Infrastructure.Email
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public SecureSocketOptions SecureSocketOptions { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}