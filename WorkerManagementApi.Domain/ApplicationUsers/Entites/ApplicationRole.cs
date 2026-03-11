using Microsoft.AspNetCore.Identity;
using WorkerManagementApi.Domain.Common.Interfaces;

namespace WorkerManagementApi.Domain.ApplicationUsers.Entites
{
    public class ApplicationRole: IdentityRole<Guid>, IEntity
    {
        public bool IsDeleted { get; set; }
        public virtual ICollection<ApplicationUserRole> Roles { get; set; }
    }
}
