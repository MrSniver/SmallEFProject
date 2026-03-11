using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;
using WorkerManagementApi.Infrastructure.Context;
using WorkerManagementApi.Infrastructure.Repositories.Common;

namespace WorkerManagementApi.Infrastructure.Repositories.ApplicationUsers
{
    public class ApplicationUserRepository: Repository<ApplicationUser>, IApplicationUserRepository
    {
        public ApplicationUserRepository(WorkerManApiContext dbContext): base(dbContext) { }

        public async Task<List<ApplicationUser>> GetUserswithRole()
        {
            return await _context.Users
                .Include(x => x.Roles).ThenInclude(x => x.Role).ToListAsync();
        }

        public async Task<ApplicationUser> GetUserWithRole(Guid id)
        {
            return await _context.Users
                .Include(x => x.Roles).ThenInclude(x => x.Role).Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<ApplicationUser>> GetUsersWithRoleWhere(Expression<Func<ApplicationUser, bool>> where)
        {
            return await _context.Users
                .Include(x => x.Roles).ThenInclude(x => x.Role).Where(where).ToListAsync();
        }
    }
}
