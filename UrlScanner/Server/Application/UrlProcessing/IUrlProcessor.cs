using System.Threading;
using System.Threading.Tasks;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.UrlProcessing
{
    internal interface IUrlProcessor
    {
        Task<UrlInfo> Process(UrlInfo urlInfo, CancellationToken stopToken);
    }
}