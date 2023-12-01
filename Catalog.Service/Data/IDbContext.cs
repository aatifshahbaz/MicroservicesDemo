using Catalog.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Service.Data
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();
    }
}
