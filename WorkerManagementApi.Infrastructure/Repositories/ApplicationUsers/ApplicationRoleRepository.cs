using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;
using WorkerManagementApi.Infrastructure.Context;
using WorkerManagementApi.Infrastructure.Repositories.Common;

namespace WorkerManagementApi.Infrastructure.Repositories.ApplicationUsers
{
    public class ApplicationRoleRepository: Repository<ApplicationRole>, IApplicationRoleRepository
    {
        public ApplicationRoleRepository(WorkerManApiContext context) : base(context) { }
    }
}
