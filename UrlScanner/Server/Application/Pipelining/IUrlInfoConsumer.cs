using System.Threading;
using System.Threading.Tasks;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.Pipelining
{
    internal interface IUrlInfoConsumer
    {
        Task Consume(UrlInfo urlInfo, CancellationToken stopToken);
        Task CompleteConsuming(CancellationToken stopToken);
    }
}