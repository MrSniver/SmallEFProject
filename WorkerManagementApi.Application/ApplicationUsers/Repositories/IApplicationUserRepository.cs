using System.Linq.Expressions;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Application.ApplicationUsers.Repositories
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        public Task<List<ApplicationUser>> GetUserswithRole();
        public Task<ApplicationUser> GetUserWithRole(Guid id);
        public Task<List<ApplicationUser>> GetUsersWithRoleWhere(Expression<Func<ApplicationUser, bool>> where);
    }
}
