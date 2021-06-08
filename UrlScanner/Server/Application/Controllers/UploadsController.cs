using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class UploadsController : Controller
    {
        private readonly UrlScanningContext _db;

        public UploadsController(UrlScanningContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        [HttpPost]
        public async Task<IActionResult> UploadFile([FromBody]List<UrlInfo> urlInfos)
        {
            await _db.UrlInfos.AddRangeAsync(urlInfos);
            await _db.SaveChangesAsync();
            
            return Ok();
        }
    }
}