using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application._Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<User> Users { get; }

        Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default, bool throwOnNotFound = false);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}