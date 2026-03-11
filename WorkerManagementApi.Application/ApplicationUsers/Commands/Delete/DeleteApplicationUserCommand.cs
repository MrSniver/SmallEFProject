using MediatR;
using Microsoft.AspNetCore.Identity;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Application.ApplicationUsers.Commands.Delete
{
    [Authorize(Roles = "admin")]
    [Authorize(Policy = "FullAccess")]
    public class DeleteApplicationUserCommand: IRequest<bool>
    {
        public Guid Id { get; set; }
    }

    public class DeleteApplicationUserCommandHandler : IRequestHandler<DeleteApplicationUserCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteApplicationUserCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<bool> Handle(DeleteApplicationUserCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(command.Id.ToString());

            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return true;
                else
                    return false;

            }
            else
                return false;
        }
    }
}
