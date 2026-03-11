using AutoMapper;
using MediatR;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Domain.Tasks.Enums;

namespace WorkerManagementApi.Application.Tasks.Commands.Update
{
    [Authorize(Roles = "admin, manager")]
    [Authorize(Policy = "Write")]
    public class UpdateTaskCommand: IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public TaskDtoStatus Status { get; set; }
        public Guid? AssignedTo { get; set; }
    }

    public class UpdateTaskCommandHandler: IRequestHandler<UpdateTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateTaskCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<bool> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.TaskRepository.GetByIdAsync(command.Id);

            if (entity == null) return false;

            if (entity.AssignedTo != command.AssignedTo)
            {
                entity.AssignedBy = _currentUserService.Id();
            }

            entity.Subject = command.Subject;
            entity.Description = command.Description;
            if (command.Status == TaskDtoStatus.Closed)
            {
                entity.FinishedDate = DateTime.UtcNow;
            }
            entity.Status = command.Status;
            entity.AssignedTo = (Guid)command.AssignedTo;

            try
            {
                _unitOfWork.BeginTransaction();
                await _unitOfWork.TaskRepository.UpdateAsync(entity);
                _unitOfWork.SaveChanges();
                _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTransaction();
                throw new Exception(ex.Message);
            }

        }
    }
}
