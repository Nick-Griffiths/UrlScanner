using System.Threading.Tasks;

namespace UrlScanner.Server.Infrastructure.Email
{
    internal interface IEmailService
    {
        public Task Send(string from, string to, string subject, string content);
    }
}