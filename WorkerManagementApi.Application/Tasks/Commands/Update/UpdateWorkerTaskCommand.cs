using AutoMapper;
using MediatR;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Domain.Tasks.Enums;

namespace WorkerManagementApi.Application.Tasks.Commands.Update
{
    [Authorize(Roles = "worker")]
    [Authorize(Policy = "Write")]
    public class UpdateWorkerTaskCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public TaskDtoStatus Status { get; set; }
        public Guid WorkerId { get; set; }

        public UpdateWorkerTaskCommand(Guid workerId) { WorkerId = workerId; }
    }

    public class UpdateWorkerTaskCommandHandler : IRequestHandler<UpdateWorkerTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateWorkerTaskCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(UpdateWorkerTaskCommand command, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.TaskRepository.GetByIdAsync(command.Id);

            if (entity == null) return false;

            entity.Status = command.Status;

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
