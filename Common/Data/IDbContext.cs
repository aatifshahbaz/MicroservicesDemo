using Microsoft.EntityFrameworkCore;

namespace Common.Data
{
    public interface IDbContext
    {
        DbSet<T> Set<T>() where T : class;
        int SaveChanges();
    }
}
