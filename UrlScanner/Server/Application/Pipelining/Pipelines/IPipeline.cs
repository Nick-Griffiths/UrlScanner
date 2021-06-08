using System.Threading;
using System.Threading.Tasks;

namespace UrlScanner.Server.Application.Pipelining.Pipelines
{
    internal interface IPipeline
    {
        public Task Execute(CancellationToken stopToken);
    }
}