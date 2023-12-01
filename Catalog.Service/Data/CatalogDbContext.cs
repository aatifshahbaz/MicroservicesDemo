using Catalog.Service.Models;
using Common.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Service.Data
{
    public class CatalogDbContext : DbContext, IDbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
    }
}
