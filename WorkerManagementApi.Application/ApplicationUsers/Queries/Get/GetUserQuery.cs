using AutoMapper;
using MediatR;
using WorkerManagementApi.Application.ApplicationUsers.Dtos;
using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Application.Common.Security;

namespace WorkerManagementApi.Application.ApplicationUsers.Queries.Get
{
    [Authorize(Roles = "admin, manager")]
    [Authorize(Policy = "Read")]
    public class GetUserQuery : IRequest<ApplicationUserDto>
    {
        public Guid Id { get; set; }

        public GetUserQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ApplicationUserDto>
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMapper _mapper;

        public GetUserQueryHandler(IApplicationUserRepository applicationUserRepository, IMapper mapper)
        {
            _applicationUserRepository = applicationUserRepository ?? throw new ArgumentNullException(nameof(applicationUserRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ApplicationUserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _applicationUserRepository.GetUserWithRole(request.Id);
            if (user == null)
                return null;
            else
                return _mapper.Map<ApplicationUserDto>(user);

        }
    }
}
