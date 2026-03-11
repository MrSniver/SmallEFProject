using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Application.Common.Services;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Application.ApplicationUsers.Commands.Update
{
    [Authorize(Roles = "admin")]
    [Authorize(Policy = "FullAccess")]
    public class UpdateAdminPasswordCommand: IRequest<bool>
    {
        public Guid Id { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmed { get; set; }
    }

    public class UpdateAdminPasswordCommandHandler: AppService, IRequestHandler<UpdateAdminPasswordCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public UpdateAdminPasswordCommandHandler(UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher,
            IUnitOfWork unitOfWork): base(unitOfWork) 
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<bool> Handle(UpdateAdminPasswordCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(command.Id.ToString());

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
