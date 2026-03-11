using WorkerManagementApi.Application.Tasks.Repositories;
using WorkerManagementApi.Domain.Tasks.Entities;
using WorkerManagementApi.Infrastructure.Context;
using WorkerManagementApi.Infrastructure.Repositories.Common;

namespace WorkerManagementApi.Infrastructure.Repositories.Tasks
{
    public class TaskRepository: Repository<TaskEntity>, ITaskRepository
    {
        public TaskRepository(WorkerManApiContext context) : base(context) { }
    }
}
