using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkerManagementApi.Application.Tokens.Commands.Create;

namespace WorkerManagementApi.Controllers
{
    [ApiController]
    [Route("connect")]
    public class TokenController: ControllerBase
    {
        private readonly IMediator _mediator;

        public TokenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateCommand request)
        {
            var response = await _mediator.Send(request);
            if (response == null) return StatusCode(403);
            return Ok(response.TokenResponse);
        }

    }
}
