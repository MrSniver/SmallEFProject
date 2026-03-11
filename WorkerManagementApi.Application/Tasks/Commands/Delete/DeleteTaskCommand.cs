using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Application.Tasks.Repositories;

namespace WorkerManagementApi.Application.Tasks.Commands.Delete
{
    [Authorize(Roles = "admin, manager")]
    [Authorize(Policy = "Delete")]
    public class DeleteTaskCommand: IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteTaskCommandHandler: IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
        {
            var task = await _unitOfWork.TaskRepository.GetByIdAsync(command.Id);

            if (task != null)
            {
                await _unitOfWork.TaskRepository.DeleteAsync(task);
                return true;
            }
            else
                return false;
        }
    }
}
