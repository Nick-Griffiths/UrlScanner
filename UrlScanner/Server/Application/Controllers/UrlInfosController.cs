using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlScanner.Server.Application.ViewModels;
using UrlScanner.Server.Infrastructure.DataAccess;

namespace UrlScanner.Server.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class UrlInfosController : Controller
    {
        private readonly UrlScanningContext _db;

        public UrlInfosController(UrlScanningContext db) => _db = db ?? throw new ArgumentNullException(nameof(db));

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var results = await _db.UrlInfos.AsNoTracking()
                .OrderBy(w => w.ScanDuration)
                .Select(w => w.ToViewModel())
                .ToArrayAsync();
            
            return Ok(results);
        }
        
        [HttpGet("lastFullScan")]
        public async Task<IActionResult> GetLastFullScan()
        {
            var lastFullScan = await _db.UrlInfos.AsNoTracking()
                .MaxAsync(r => r.LastTimeScanned);
        
            return Ok(new {LastFullScan = lastFullScan?.ToString("G") ?? "N/A"});
        }
    }
}