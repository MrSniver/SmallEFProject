using AutoMapper;
using MediatR;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Application.Tasks.Dtos;
using WorkerManagementApi.Application.Tasks.Repositories;
using WorkerManagementApi.Domain.Tasks.Entities;
using WorkerManagementApi.Domain.Tasks.Enums;

namespace WorkerManagementApi.Application.Tasks.Commands.Create
{
    [Authorize(Roles = "admin, manager")]
    [Authorize(Policy = "Write")]
    public class CreateTaskCommand: IRequest<Guid>
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public TaskDtoStatus Status { get; set; }
        public Guid AssignedTo { get; set; }
    }

    public class CreateTaskCommandHandler: IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper, ICurrentUserService currentUserService, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Guid> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
        {
            var taskDto = new TaskDto
            {
                Subject = command.Subject,
                Description = command.Description,
                Status = command.Status,
                CreatedDate = DateTime.UtcNow,
                AssignedTo = command.AssignedTo,
                AssignedBy = _currentUserService.Id(),
            };

            _unitOfWork.BeginTransaction();
            var result = await _taskRepository.CreateAsync(_mapper.Map<TaskEntity>(taskDto));
            _unitOfWork.SaveChanges();
            _unitOfWork.CommitTransaction();

            return result.Id;
        }
    }
}
