using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Domain.Tasks.Entities;

namespace WorkerManagementApi.Application.Tasks.Repositories
{
    public interface ITaskRepository: IRepository<TaskEntity>
    {
    }
}
