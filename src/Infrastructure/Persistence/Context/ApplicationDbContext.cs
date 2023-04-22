using Application._Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        private readonly AuditedEntitySaveChangesInterceptor _auditedEntitySaveChangesInterceptor;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
                                    AuditedEntitySaveChangesInterceptor auditedEntitySaveChangesInterceptor)
                                    : base(options)
        {
            _auditedEntitySaveChangesInterceptor = auditedEntitySaveChangesInterceptor;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.AddInterceptors(_auditedEntitySaveChangesInterceptor);
        }

        public async Task DeleteAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default, bool throwOnEntityNotFound = false)
        {
            try
            {
                ArgumentNullException.ThrowIfNull(entity);

                EntityEntry track = Attach(entity);
                track.State = EntityState.Deleted;
                await SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (throwOnEntityNotFound)
                    throw;
            }
        }

        public DbSet<User> Users => Set<User>();
    }
}