using Catalog.Service.Models;
using Common.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Service.Data
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }
    }
}
