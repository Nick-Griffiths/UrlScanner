using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UrlScanner.Server.Infrastructure.DataAccess;
using static System.Threading.Tasks.Task;

namespace UrlScanner.Server.Application.Pipelining
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal sealed class EfCoreUrlInfoProviderConsumer : IUrlInfoProvider, IUrlInfoConsumer
    {
        private readonly UrlScanningContext _db;

        internal EfCoreUrlInfoProviderConsumer(UrlScanningContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public IEnumerable<UrlInfo> GetUrlInfos() => _db.UrlInfos.AsEnumerable();

        public Task Consume(UrlInfo urlInfo, CancellationToken stopToken) => CompletedTask;
        public Task CompleteConsuming(CancellationToken stopToken) => _db.SaveChangesAsync(stopToken);
    }
}