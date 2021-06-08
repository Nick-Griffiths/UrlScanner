using System.Threading;
using System.Threading.Tasks;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.UrlLoading
{
    internal interface IUrlLoader
    {
        Task<string> Load(UrlInfo urlInfo, CancellationToken stopToken);
    }
}