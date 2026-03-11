using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Tokens.Dtos;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Infrastructure.Services
{
    public class CurrentUserService: ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenDto _tokenDto;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IOptions<TokenDto> tokenDto)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenDto = tokenDto.Value;
        }

        public Guid Id()
        {
            if (_httpContextAccessor.HttpContext == null) return Guid.Empty;
            if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]))
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
                try
                {
                    var userId = Guid.Parse(jwtSecurityToken.Claims.First(x => x.Type == "UserId").Value);
                    return userId;
                }
                catch
                {
                    return Guid.Empty;
                }
            }
            else
                return Guid.Empty;
        }

        public string Username()
        {
            if (_httpContextAccessor.HttpContext == null) return "";
            if (!String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["Authorization"]))
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
                try
                {
                    var userId = jwtSecurityToken.Claims.First(x => x.Type == "UserName").Value;
                    return userId;
                }
                catch
                {
                    return "";
                }
            }
            else
                return "";
        }
    }
}
