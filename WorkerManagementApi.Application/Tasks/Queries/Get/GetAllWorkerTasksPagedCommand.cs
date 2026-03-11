using AutoMapper;
using MediatR;
using WorkerManagementApi.Application.Common.Extensions;
using WorkerManagementApi.Application.Common.Pagination;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Application.Tasks.Dtos;
using WorkerManagementApi.Application.Tasks.Repositories;
using WorkerManagementApi.Application.Tasks.Services;

namespace WorkerManagementApi.Application.Tasks.Queries.Get
{
    [Authorize(Roles = "worker")]
    [Authorize(Policy = "Read")]
    public class GetAllWorkerTasksPagedCommand: IRequest<PagedResult<TaskDto>>
    {
        public int Index { get; set; }
        public int PageSize { get; set; }
        public TaskFilterDto Filters { get; set; }
        public Sorting Sorting { get; set; }
        public Guid WorkerId { get; set; }

        public GetAllWorkerTasksPagedCommand(Guid workerId) { WorkerId = workerId; }
    }

    public class GetAllWorkerTasksPagedCommandHandler: IRequestHandler<GetAllWorkerTasksPagedCommand, PagedResult<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public GetAllWorkerTasksPagedCommandHandler(ITaskRepository taskRepository, ITaskService taskService, IMapper mapper)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<PagedResult<TaskDto>> Handle(GetAllWorkerTasksPagedCommand request, CancellationToken cancellationToken)
        {
            if (request.PageSize == 0) return null;

            var where = _taskService.GetWhereExpression(request.Filters).And(x => x.AssignedTo == request.WorkerId);
            var order = _taskService.GetOrderExpression(request.Sorting);

            var entityList = await _taskRepository.GetWhereAsync(where);

            if (entityList.Count() == 0)
            {
                var nullList = new List<TaskDto>();
                return nullList.GetPaged(request.Index, request.PageSize);
            }

            return request.Sorting.SortAscending ? _mapper.Map<List<TaskDto>>(entityList.AsQueryable().OrderBy(order)).GetPaged(request.Index, request.PageSize)
                : _mapper.Map<List<TaskDto>>(entityList.AsQueryable().OrderByDescending(order)).GetPaged(request.Index, request.PageSize);
        }
    }
}
