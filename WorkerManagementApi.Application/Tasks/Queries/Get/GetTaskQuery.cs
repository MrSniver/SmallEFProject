using AutoMapper;
using MediatR;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Application.Tasks.Dtos;
using WorkerManagementApi.Application.Tasks.Repositories;

namespace WorkerManagementApi.Application.Tasks.Queries.Get
{
    [Authorize(Roles = "admin, manager")]
    [Authorize(Policy = "Read")]
    public class GetTaskQuery: IRequest<TaskDto>
    {
        public Guid Id { get; set; }
        public GetTaskQuery(Guid id) { Id = id; }
    }

    public class GetTaskQueryHandler: IRequestHandler<GetTaskQuery, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetTaskQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TaskDto> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            var entity = await _taskRepository.GetByIdAsync(request.Id);

            if (entity == null) return null;

            return _mapper.Map<TaskDto>(entity);
        }
    }
}
