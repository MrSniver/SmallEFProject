using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Infrastructure.ModelBuilders
{
    public class ApplicationUserRoleBuilder: IEntityTypeConfiguration<ApplicationUserRole>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
        {
            builder.ToTable("AspNetUserRoles");

            builder.HasQueryFilter(x => x.IsDeleted == false);
        }
    }
}
