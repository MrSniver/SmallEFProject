using WorkerManagementApi.Application.Common.Pagination;

namespace WorkerManagementApi.Application.ApplicationUsers.Dtos
{
    public class ApplicationUserPagedDto
    {
        public PagedResult<ApplicationUserDto> ApplicationUsers { get; set; }
        public ApplicationUserFilterDto Filters { get; set; }
        public Sorting Sorting { get; set; }
    }
}
