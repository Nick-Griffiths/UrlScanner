using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.UrlLoading
{
    internal sealed class UrlLoader : IUrlLoader
    {
        private const string Scheme = "http";
        
        private readonly HttpClient _httpclient;

        public UrlLoader(HttpClient httpclient)
        {
            _httpclient = httpclient ?? throw new ArgumentNullException(nameof(httpclient));
        }

        public async Task<string> Load(UrlInfo urlInfo, CancellationToken stopToken)
        {
            var url = new UriBuilder(Scheme, urlInfo.Url).ToString();
            var response = await _httpclient.GetAsync(url, stopToken);
            
            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsStringAsync(stopToken)
                : string.Empty;
        }
    }
}