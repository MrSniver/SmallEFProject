using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Security;
using WorkerManagementApi.Application.Common.Services;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;
using WorkerManagementApi.Domain.ApplicationUsers.Interfaces;

namespace WorkerManagementApi.Application.ApplicationUsers.Commands.Update
{
    [Authorize(Roles = "admin")]
    [Authorize(Policy = "FullAccess")]
    public class UpdateApplicationUserCommand: IRequest<IdentityResult>, IApplicationUser
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public Guid? ManagerId { get; set; }
        public string Email { get; set; }
        public string  Role { get; set; }

    }

    public class UpdateApplicationUserCommandHandler: AppService, IRequestHandler<UpdateApplicationUserCommand, IdentityResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IMapper _mapper;

        public UpdateApplicationUserCommandHandler(UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher, 
            RoleManager<ApplicationRole> roleManager,IApplicationUserRepository applicationUserRepository, IMapper mapper,
            IUnitOfWork unitOfWork): base(unitOfWork)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _applicationUserRepository = applicationUserRepository ?? throw new ArgumentNullException(nameof(_applicationUserRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<IdentityResult> Handle(UpdateApplicationUserCommand command, CancellationToken cancellationToken)
        {
            ApplicationUser user = await _applicationUserRepository.GetUserWithRole(command.Id);

            if (user == null) return null;
            
            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.ManagerId = command.ManagerId;
            user.Email = command.Email;
            user.UserName = command.UserName;
            UnitOfWork.BeginTransaction();

            IdentityResult result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                List<string> roleList = new List<string>() { user.Roles.Select(x => x.Role).FirstOrDefault().ToString() };
                await _userManager.RemoveFromRolesAsync(user, roleList);
                await _userManager.AddToRolesAsync(user, new[] { _roleManager.FindByNameAsync(command.Role).Result.Name });

                UnitOfWork.SaveChanges();
                UnitOfWork.CommitTransaction();
            }
            else
                UnitOfWork.RollbackTransaction();

            return result;
        }
    }
}
