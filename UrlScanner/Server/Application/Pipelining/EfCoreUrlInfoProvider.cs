using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.Pipelining
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    internal sealed class EfCoreUrlInfoProvider : IUrlInfoProvider
    {
        private readonly UrlScanningContext _db;

        internal EfCoreUrlInfoProvider(UrlScanningContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        
        public IEnumerable<UrlInfo> GetUrlInfos() => _db.UrlInfos.AsNoTracking().AsEnumerable();
    }
}