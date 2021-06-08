using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.UrlProcessing.Scanners
{
    internal interface IUrlScanner
    {
        public void Scan(string htmlContent, UrlInfo urlInfo);
    }
}