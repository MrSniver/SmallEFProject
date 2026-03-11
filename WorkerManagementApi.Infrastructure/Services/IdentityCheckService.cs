using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Infrastructure.Services
{
    public class IdentityCheckService: IIdentityCheckService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityCheckService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(x => x.Id == userId);

            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}
