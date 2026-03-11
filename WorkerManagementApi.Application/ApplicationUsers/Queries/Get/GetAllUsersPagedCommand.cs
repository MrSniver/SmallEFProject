using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using WorkerManagementApi.Application.ApplicationUsers.Dtos;
using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Application.ApplicationUsers.Services;
using WorkerManagementApi.Application.Common.Pagination;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Application.ApplicationUsers.Queries.Get
{
    [Authorize(Roles = "admin, manager")]
    [Authorize(Policy = "Read")]
    public class GetAllUsersPagedCommand : IRequest<PagedResult<ApplicationUserDto>>
    {
        public int Index { get; set; }
        public int PageSize { get; set; }
        public ApplicationUserFilterDto Filters { get; set; }
        public Sorting Sorting { get; set; }
    }

    public class GetAllUsersPagedCommandHandler : IRequestHandler<GetAllUsersPagedCommand, PagedResult<ApplicationUserDto>>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IApplicationUserService _applicationUserService;
        private readonly IMapper _mapper;

        public GetAllUsersPagedCommandHandler(IApplicationUserRepository applicationUserRepository, IApplicationUserService applicationUserService, IMapper mapper)
        {
            _applicationUserRepository = applicationUserRepository ?? throw new ArgumentNullException(nameof(applicationUserRepository));
            _applicationUserService = applicationUserService ?? throw new ArgumentNullException(nameof(applicationUserService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResult<ApplicationUserDto>> Handle(GetAllUsersPagedCommand request, CancellationToken cancellationToken)
        {
            if (request.PageSize == 0) return null;

            var where = _applicationUserService.GetWhereExpression(request.Filters);
            var order = _applicationUserService.GetOrderExpression(request.Sorting);

            var applicationUserList = await _applicationUserRepository.GetUsersWithRoleWhere(where);

            if (applicationUserList.Count() == 0)
            {
                var nullList = new List<ApplicationUserDto>();
                return nullList.GetPaged(request.Index, request.PageSize);
            }

            return request.Sorting.SortAscending ? _mapper.Map<List<ApplicationUserDto>>(applicationUserList.AsQueryable().OrderBy(order)).GetPaged(request.Index, request.PageSize)
                : _mapper.Map<List<ApplicationUserDto>>(applicationUserList.AsQueryable().OrderByDescending(order)).GetPaged(request.Index, request.PageSize);
        }
    }
}
