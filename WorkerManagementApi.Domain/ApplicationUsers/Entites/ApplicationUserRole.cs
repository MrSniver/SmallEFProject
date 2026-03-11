using Microsoft.AspNetCore.Identity;
using WorkerManagementApi.Domain.Common.Interfaces;

namespace WorkerManagementApi.Domain.ApplicationUsers.Entites
{
    public class ApplicationUserRole: IdentityUserRole<Guid>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
        public bool IsDeleted { get; set; }
    }
}
