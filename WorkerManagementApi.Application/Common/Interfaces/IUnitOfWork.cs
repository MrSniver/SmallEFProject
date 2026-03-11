using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Application.Tasks.Repositories;

namespace WorkerManagementApi.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IApplicationUserRepository ApplicationUserRepository { get; }
        IApplicationRoleRepository ApplicationRoleRepository { get; }
        ITaskRepository TaskRepository { get; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void SaveChanges();
    }
}
