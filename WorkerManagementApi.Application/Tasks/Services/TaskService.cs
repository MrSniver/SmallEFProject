using System.Linq.Expressions;
using WorkerManagementApi.Application.Common.Extensions;
using WorkerManagementApi.Application.Common.Pagination;
using WorkerManagementApi.Application.Tasks.Dtos;
using WorkerManagementApi.Domain.Tasks.Entities;

namespace WorkerManagementApi.Application.Tasks.Services
{
    public class TaskService : ITaskService
    {
        public Expression<Func<TaskEntity, bool>> GetWhereExpression(TaskFilterDto filter)
        {
            Expression<Func<TaskEntity, bool>> where = x => x.IsDeleted == false;

            if (filter.Status == null && filter.CreatedDate == null && filter.FinishedDate == null
                && filter.AssignedTo == null && filter.AssignedBy == null)
                    return where;

            if (filter.Status != null)
            {
                where = where.And(x => x.Status == filter.Status);
            }
            if (filter.CreatedDate != null)
            {
                where = where.And(x => x.CreatedDate == filter.CreatedDate);
            }
            if (filter.FinishedDate != null)
            {
                where = where.And(x => x.FinishedDate == filter.FinishedDate);
            }
            if (filter.AssignedTo != null) 
            {
                where = where.And(x => x.AssignedTo == filter.AssignedTo);
            }
            if (filter.AssignedBy != null) 
            {
                where = where.And(x => x.AssignedBy == filter.AssignedBy);
            }

            return where;
        }

        public Expression<Func<TaskEntity, object>> GetOrderExpression(Sorting sorting)
        {
            if (sorting == null) return null;

            switch (sorting.SortParam)
            {
                case "Status":
                    return x => x.Status;
                case "FinishedDate":
                    return x => x.FinishedDate;
                case "AssignedTo":
                    return x => x.AssignedTo;
                case "AssignedBy":
                    return x => x.AssignedBy;
                default:
                    return x => x.CreatedDate;
            }
        }
    }
}
