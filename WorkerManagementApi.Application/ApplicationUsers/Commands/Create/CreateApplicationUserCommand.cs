using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Application.Common.Services;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;
using WorkerManagementApi.Domain.ApplicationUsers.Interfaces;

namespace WorkerManagementApi.Application.ApplicationUsers.Commands.Create
{
    [Authorize(Roles = "admin")]
    [Authorize(Policy = "FullAccess")]
    public class CreateApplicationUserCommand: IRequest<IdentityResult>, IApplicationUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }
        public Guid? ManagerId { get; set; }
        public string Role { get; set; }
    }

    public class CreateApplicationUserCommandHandler: AppService, IRequestHandler<CreateApplicationUserCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IMapper _mapper;
        
        public CreateApplicationUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, 
           IPasswordHasher<ApplicationUser> passwordHasher, IMapper mapper, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _mapper = mapper;
        }

        public async Task<IdentityResult> Handle(CreateApplicationUserCommand command, CancellationToken cancellationToken)
        {
            UnitOfWork.BeginTransaction();

            ApplicationUser user = new ApplicationUser
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                UserName = command.UserName,
                ManagerId = command.ManagerId,
                PasswordHash = command.Password,
                EmailConfirmed = true,
                TwoFactorEnabled = false
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, user.PasswordHash);

            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, new[] { _roleManager.FindByNameAsync(command.Role).Result.Name });
                UnitOfWork.SaveChanges();
                UnitOfWork.CommitTransaction();
                return result;
            }
            else
            {
                UnitOfWork.RollbackTransaction();
                return result;
            }
        }
    }
}
