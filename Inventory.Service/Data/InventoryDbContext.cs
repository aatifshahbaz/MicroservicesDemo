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

        public DbSet<InventoryItem> InventoryItems { get; set; }
    }
}
