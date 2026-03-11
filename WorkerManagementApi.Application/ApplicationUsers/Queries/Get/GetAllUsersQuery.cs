using AutoMapper;
using MediatR;
using WorkerManagementApi.Application.ApplicationUsers.Dtos;
using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Application.Common.Security;

namespace WorkerManagementApi.Application.ApplicationUsers.Queries.Get
{
    [Authorize(Roles = "admin, manager")]
    [Authorize(Policy = "Read")]
    public class GetAllUsersQuery : IRequest<List<ApplicationUserDto>>
    {
        public GetAllUsersQuery() { }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<ApplicationUserDto>>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMapper _mapper;

        public GetAllUsersQueryHandler(IApplicationUserRepository applicationUserRepository, IMapper mapper)
        {
            _applicationUserRepository = applicationUserRepository ?? throw new ArgumentNullException(nameof(applicationUserRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<ApplicationUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _applicationUserRepository.GetUserswithRole();
            if (users == null)
                return null;
            else
            {
                return _mapper.Map<List<ApplicationUserDto>>(users);
            }
        }
    }
}
