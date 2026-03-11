using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Tasks.Repositories;
using WorkerManagementApi.Infrastructure.Context;

namespace WorkerManagementApi.Infrastructure.Repositories.Common
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly WorkerManApiContext _context;

        public UnitOfWork(
            IApplicationUserRepository applicationUserRepository,
            IApplicationRoleRepository applicationRoleRepository, 
            ITaskRepository taskRepository,
            WorkerManApiContext context)
        {
            ApplicationRoleRepository = applicationRoleRepository;
            ApplicationUserRepository = applicationUserRepository;
            TaskRepository = taskRepository;
            _context = context;
        }

        public IApplicationRoleRepository ApplicationRoleRepository { get; }
        public IApplicationUserRepository ApplicationUserRepository { get; }
        public ITaskRepository TaskRepository { get; }

        public void BeginTransaction()
        {
            _context.Database.BeginTransactionAsync();
        }

        public void CommitTransaction() 
        {
            _context.Database.CommitTransactionAsync();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransactionAsync();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }


    }
}
