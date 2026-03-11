using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkerManagementApi.Application.ApplicationUsers.Commands.Create;
using WorkerManagementApi.Application.ApplicationUsers.Commands.Delete;
using WorkerManagementApi.Application.ApplicationUsers.Commands.Update;
using WorkerManagementApi.Application.ApplicationUsers.Queries.Get;
using WorkerManagementApi.Application.Common.Interfaces;

namespace WorkerManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationUserController: ControllerBase
    {
        private IIdentityCheckService _identityCheckService;
        private ICurrentUserService _currentUserService;
        private IMediator _mediator;

        public ApplicationUserController(IMediator mediator, IIdentityCheckService identityService, ICurrentUserService currentUserService)
        {
            _identityCheckService = identityService;
            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetUserQuery(id));
            if (result == null) 
                return NotFound();
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = _currentUserService.Id();
            if (userId == Guid.Empty) return Unauthorized();

            var isAdmin = await _identityCheckService.IsInRoleAsync(userId, "admin");
            var isManager = await _identityCheckService.IsInRoleAsync(userId, "manager");

            if (isAdmin || isManager)
            {
                var result = await _mediator.Send(new GetAllUsersQuery());
                if (result == null) return NotFound();
                return Ok(result);
            }
            else
                return Unauthorized();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("paged/{index}")]
        public async Task<IActionResult> GetPagedApplicationUsers(int index, GetAllUsersPagedCommand request)
        {
            var userId = _currentUserService.Id();
            if (userId == Guid.Empty) return Unauthorized();

            if (request != null) request.Index = index;

            var isAdmin = await _identityCheckService.IsInRoleAsync(userId, "admin");
            var isManager = await _identityCheckService.IsInRoleAsync(userId, "manager");

            if (isAdmin || isManager)
            {
                var result = await _mediator.Send(request);
                if (result == null) return NotFound();
                return Ok(result);
            }
            else
                return Unauthorized();
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateApplicationUserCommand command)
        {
            var userId = _currentUserService.Id();
            if (userId == Guid.Empty) return Unauthorized();

            var isAdmin = await _identityCheckService.IsInRoleAsync(userId, "admin");
            if (!isAdmin) return Unauthorized();

            var result = await _mediator.Send(command);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            else
                return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateApplicationUserCommand command)
        {
            var userId = _currentUserService.Id();
            if (userId == Guid.Empty) return Unauthorized();

            var isAdmin = await _identityCheckService.IsInRoleAsync(userId, "admin");
            if (!isAdmin) return Unauthorized();

            var result = await _mediator.Send(command);
            if (!result.Succeeded)
                return NotFound(result.Errors);
            else
                return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        [Route("changepassword/otherUser")]
        public async Task<ActionResult<bool>> UpdateAdministratorPassword(UpdateAdminPasswordCommand command)
        {

            var userId = _currentUserService.Id();
            if (userId == Guid.Empty) return Unauthorized();

            var isAdmin = await _identityCheckService.IsInRoleAsync(userId, "admin");
            if (!isAdmin) return Unauthorized();

            var result = await _mediator.Send(command);
            if (result == false)
                return BadRequest();
            else
                return Ok(result);
        }
        [AllowAnonymous]
        [HttpPut]
        [Route("changepassword")]
        public async Task<ActionResult<bool>> UpdateUserPassword(UpdateUserPasswordCommand command)
        {
            var result = await _mediator.Send(command);
            if (result == false)
                return NotFound();
            else
                return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = _currentUserService.Id();
            if (userId == Guid.Empty) return Unauthorized();

            var isAdmin = await _identityCheckService.IsInRoleAsync(userId, "admin");
            if (isAdmin)
            {
                var result = await _mediator.Send(new DeleteApplicationUserCommand { Id = id });
                if (result == false) return NotFound();
                else return Ok(result);

            }
            else
                return Unauthorized();
        }
    }
}
