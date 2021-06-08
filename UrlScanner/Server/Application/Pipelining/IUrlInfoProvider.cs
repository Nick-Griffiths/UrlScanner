using System.Collections.Generic;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.Pipelining
{
    internal interface IUrlInfoProvider
    {
        IEnumerable<UrlInfo> GetUrlInfos();
    }
}