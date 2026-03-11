using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Application.Tasks.Dtos;
using WorkerManagementApi.Application.Tasks.Repositories;

namespace WorkerManagementApi.Application.Tasks.Queries.Get
{
    [Authorize(Roles = "worker")]
    [Authorize(Policy = "Read")]
    public class GetAllWorkerTasksQuery: IRequest<List<TaskDto>>
    {
        public Guid WorkerId { get; set; }
        public GetAllWorkerTasksQuery(Guid workerId) { WorkerId = workerId; }
    }

    public class GetAllWorkerTasksQueryHandler: IRequestHandler<GetAllWorkerTasksQuery, List<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetAllWorkerTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<TaskDto>> Handle(GetAllWorkerTasksQuery request, CancellationToken cancellationToken)
        {
            var entityList = await _taskRepository.GetWhereAsync(x => x.AssignedTo == request.WorkerId);

            if (entityList == null || entityList.Count() == 0) return null;
            
            return _mapper.Map<List<TaskDto>>(entityList);
        }
    }
}
