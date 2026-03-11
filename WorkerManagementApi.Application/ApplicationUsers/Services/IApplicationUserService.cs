using System.Linq.Expressions;
using WorkerManagementApi.Application.ApplicationUsers.Dtos;
using WorkerManagementApi.Application.Common.Pagination;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Application.ApplicationUsers.Services
{
    public interface IApplicationUserService
    {
        Expression<Func<ApplicationUser, bool>> GetWhereExpression(ApplicationUserFilterDto userFilterDto);

        Expression<Func<ApplicationUser, object>> GetOrderExpression(Sorting sorting);
    }
}
