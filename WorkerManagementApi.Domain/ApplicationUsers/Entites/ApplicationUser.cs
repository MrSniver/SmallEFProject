using Microsoft.AspNetCore.Identity;
using WorkerManagementApi.Domain.ApplicationUsers.Interfaces;
using WorkerManagementApi.Domain.Common.Interfaces;

namespace WorkerManagementApi.Domain.ApplicationUsers.Entites
{
    public class ApplicationUser: IdentityUser<Guid>, IEntity, IApplicationUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public Guid? ManagerId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public Guid LastModifiedBy { get; set; }

        public virtual ICollection<IdentityUserClaim<Guid>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<Guid>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<Guid>> Tokens { get; set; }
        public virtual ICollection<ApplicationUserRole> Roles { get; set; }
    }
}
