using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Application.Tasks.Dtos;
using WorkerManagementApi.Application.Tasks.Repositories;

namespace WorkerManagementApi.Application.Tasks.Queries.Get
{
    [Authorize(Roles = "admin, manager")]
    [Authorize(Policy = "Read")]
    public class GetAllTasksQuery: IRequest<List<TaskDto>>
    {
        public GetAllTasksQuery() { }
    }

    public class GetAllTasksQueryHandler: IRequestHandler<GetAllTasksQuery, List<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<List<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            var entityList = await _taskRepository.ListAllAsync();

            if (entityList == null || entityList.Count == 0) return null;

            return _mapper.Map<List<TaskDto>>(entityList);
        }
    }
}
