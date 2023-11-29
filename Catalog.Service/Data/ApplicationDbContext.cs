using Catalog.Service.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Catalog.Service.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Item> Items { get; set; }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    var entitiesAssembly = Assembly.GetEntryAssembly(); // typeof(IEntity).Assembly;
        //    modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
        //}


    }
}
