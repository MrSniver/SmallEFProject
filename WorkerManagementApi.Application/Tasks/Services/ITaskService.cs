using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.Common.Pagination;
using WorkerManagementApi.Application.Tasks.Dtos;
using WorkerManagementApi.Domain.Tasks.Entities;

namespace WorkerManagementApi.Application.Tasks.Services
{
    public interface ITaskService
    {
        Expression<Func<TaskEntity, bool>> GetWhereExpression(TaskFilterDto filter);

        Expression<Func<TaskEntity, object>> GetOrderExpression(Sorting sorting);
    }
}
