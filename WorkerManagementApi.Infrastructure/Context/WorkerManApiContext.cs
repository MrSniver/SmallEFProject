using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using System.Reflection;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;
using WorkerManagementApi.Domain.Common.Entities;
using WorkerManagementApi.Domain.Tasks.Entities;

namespace WorkerManagementApi.Infrastructure.Context
{
    public class WorkerManApiContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid, IdentityUserClaim<Guid>,
        ApplicationUserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IWorkerManApiContext
    {
        private readonly ICurrentUserService _currentUserService;

        public WorkerManApiContext(DbContextOptions options,
            ICurrentUserService currentUserService): base(options)
        {
            _currentUserService = currentUserService;
        }

        public DbSet<TaskEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (EntityEntry<AuditableEntity> entry in ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property(x => x.CreatedOn).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.CreatedBy).CurrentValue = _currentUserService.Id();
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(x => x.LastModifiedOn).CurrentValue = DateTime.UtcNow;
                    entry.Property(x => x.LastModifiedBy).CurrentValue= _currentUserService.Id();
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}
