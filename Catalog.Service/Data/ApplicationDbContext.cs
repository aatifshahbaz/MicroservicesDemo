using Catalog.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Service.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
    }
}
