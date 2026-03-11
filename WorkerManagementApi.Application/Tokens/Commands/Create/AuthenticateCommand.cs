using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using WorkerManagementApi.Application.Tokens.Dtos;
using WorkerManagementApi.Application.Tokens.Interfaces;

namespace WorkerManagementApi.Application.Tokens.Commands.Create
{
    public class AuthenticateCommand: TokenRequestDto, IRequest<CommandResponse>
    {
    }

    public class CommandResponse
    {
        public TokenResponseDto TokenResponse { get; set; }
    }

    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, CommandResponse>
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly HttpContext _httpContext;

        public AuthenticateCommandHandler(ITokenService tokenService, IMapper mapper, IHttpContextAccessor httpContext)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _httpContext = httpContext.HttpContext;
        }

        public async Task<CommandResponse> Handle(AuthenticateCommand command, CancellationToken cancellationToken)
        {
            CommandResponse response = new CommandResponse();

            TokenResponseDto tokenResponse = await _tokenService.Authenticate(command);

            if (tokenResponse == null)
                return null;

            response.TokenResponse = tokenResponse;
            return response;
        }
    }
}
