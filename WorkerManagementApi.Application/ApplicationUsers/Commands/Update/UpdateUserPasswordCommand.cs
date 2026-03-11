using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Application.Common.Services;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Application.ApplicationUsers.Commands.Update
{
    [Authorize(Roles = "admin, manager, worker")]
    [Authorize(Policy = "Write")]
    public class UpdateUserPasswordCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmed { get; set; }
    }

    public class UpdateUserPasswordCommandHandler : AppService, IRequestHandler<UpdateUserPasswordCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserPasswordCommandHandler(UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher,
            ICurrentUserService currentUserService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<bool> Handle(UpdateUserPasswordCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(_currentUserService.Id().ToString());

            if (user == null) return false;

            if (command.NewPassword != command.NewPasswordConfirmed) throw new InvalidCredentialException("Passwords do not match");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, command.NewPassword);

            if (result.Succeeded)
                return true;
            else
                return false;
        }
    }
}
