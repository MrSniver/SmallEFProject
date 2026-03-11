using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Tasks.Commands.Create;
using WorkerManagementApi.Application.Tasks.Commands.Delete;
using WorkerManagementApi.Application.Tasks.Commands.Update;
using WorkerManagementApi.Application.Tasks.Queries.Get;

namespace WorkerManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly IIdentityCheckService _identityCheckService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;

        public TaskController(IIdentityCheckService identityCheckService, ICurrentUserService currentUserService, IMediator mediator)
        {
            _identityCheckService = identityCheckService;
            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetTaskQuery(id));
            if (result == null) return NotFound();
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
                var result = await _mediator.Send(new GetAllTasksQuery());
                if (result == null) return NotFound();
                return Ok(result);
            }
            else
            {
                var workerRequest = new GetAllWorkerTasksQuery(userId);

                var result = await _mediator.Send(workerRequest);
                if (result == null) return NotFound();
                return Ok(result);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("paged/{index}")]
        public async Task<IActionResult> GetPagedTasks(int index, GetAllTasksPagedCommand request)
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
            {
                var workerRequest = new GetAllWorkerTasksPagedCommand(userId)
                {
                    Filters = request.Filters,
                    Sorting = request.Sorting,
                    Index = request.Index,
                    PageSize = request.PageSize,
                };

                var result = await _mediator.Send(workerRequest);
                if (result == null) return NotFound();
                return Ok(result);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskCommand command)
        {
            var userId = _currentUserService.Id();
            if (userId == Guid.Empty) return Unauthorized();

            var isAdmin = await _identityCheckService.IsInRoleAsync(userId, "admin");
            var isManager = await _identityCheckService.IsInRoleAsync(userId, "manager");

            if (isAdmin || isManager)
            {
                var result = await _mediator.Send(command);
                if (result == Guid.Empty)
                    return BadRequest();
                else
                    return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
            }
            else
                return Unauthorized();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateTaskCommand command)
        {
            var userId = _currentUserService.Id();
            if (userId == Guid.Empty) return Unauthorized();

            var isAdmin = await _identityCheckService.IsInRoleAsync(userId, "admin");
            var isManager = await _identityCheckService.IsInRoleAsync(userId, "manager");

            if (isAdmin || isManager)
            {
                var result = await _mediator.Send(command);
                if (result == false)
                    return NotFound();
                else
                    return new ObjectResult(result) { StatusCode = StatusCodes.Status200OK };
            }
            else
            {
                var workerCommand = new UpdateWorkerTaskCommand(userId)
                {
                    Id = command.Id,
                    Status = command.Status
                };

                var result = await _mediator.Send(workerCommand);
                if (result == false)
                    return NotFound();
                else
                    return new ObjectResult(result) { StatusCode = StatusCodes.Status200OK };
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var userId = _currentUserService.Id();
            if (userId == Guid.Empty) return Unauthorized();

            var isAdmin = await _identityCheckService.IsInRoleAsync(userId, "admin");
            var isManager = await _identityCheckService.IsInRoleAsync(userId, "manager");

            if (isAdmin || isManager)
            {
                var result = await _mediator.Send(new DeleteTaskCommand { Id = id });
                if (result == false)
                    return NotFound();
                else
                    return Ok();
            }
            else
                return Unauthorized();
        }
    }
}
