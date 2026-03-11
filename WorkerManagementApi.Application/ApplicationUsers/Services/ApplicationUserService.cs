using System.Linq.Expressions;
using WorkerManagementApi.Application.ApplicationUsers.Dtos;
using WorkerManagementApi.Application.Common.Extensions;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Pagination;
using WorkerManagementApi.Application.Common.Services;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Application.ApplicationUsers.Services
{
    public class ApplicationUserService: AppService,  IApplicationUserService
    {
        public ApplicationUserService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        public Expression<Func<ApplicationUser, bool>> GetWhereExpression(ApplicationUserFilterDto filter)
        {
            Expression<Func<ApplicationUser, bool>> where = x => x.IsDeleted == false;

            if (filter == null)
                return where;

            if ((filter.UserName == null || filter.UserName == String.Empty) && (filter.LastName == null || filter.LastName == String.Empty)
                && (filter.FirstName == null || filter.FirstName == String.Empty) && (filter.Role == null || filter.Role == String.Empty)
                && (filter.ManagerId == null || filter.ManagerId == Guid.Empty))
                return where;

            if (filter.UserName != null)
            {
                where = where.And(x => x.UserName.ToUpper().Contains(filter.UserName.ToUpper()));
            }
            if (filter.LastName != null) 
            {
                where = where.And(x => x.LastName.ToUpper().Contains(filter.LastName.ToUpper()));
            }
            if (filter.FirstName != null)
            {
                where = where.And(x => x.FirstName.ToUpper().Contains(filter.FirstName.ToUpper()));
            }
            if (filter.Role != null) 
            {
                where = where.And(x => x.Roles.Any(x => x.Role.Name.ToUpper() == filter.Role.ToUpper()));
            }
            if (filter.ManagerId != null)
            {
                where = where.And(x => x.ManagerId == filter.ManagerId);
            }

            return where;
        }

        public Expression<Func<ApplicationUser, object>> GetOrderExpression(Sorting sorting)
        {
            if (sorting is null)
                return null;

            switch (sorting.SortParam)
            {
                case "UserName":
                    return x => x.UserName;
                case "LastName":
                    return x => x.LastName;
                case "FirstName":
                    return x => x.FirstName;
                default:
                    return x => x.UserName;
            }
        }
    }
}
