using Common.Data;
using Inventory.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Service.Data
{
    public class InventoryDbContext : DbContext, IDbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {

        }

        //Set<T> and SaveChangesAsync are left here intentionaly, they belong to IDbContext
        //but they have no effect as CatalogDbContext haven't implemented them and its fine
        public DbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }
    }
}
