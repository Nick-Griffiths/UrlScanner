using Microsoft.EntityFrameworkCore;

namespace UrlScanner.Server.Infrastructure.DataAccess
{
    public sealed class UrlScanningContext : DbContext
    {
        internal DbSet<UrlInfo> UrlInfos { get; set; }
        
        public UrlScanningContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UrlInfo>().HasAlternateKey(w => new { w.Name, w.Url });
        }
    }
}