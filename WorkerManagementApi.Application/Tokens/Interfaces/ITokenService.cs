using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerManagementApi.Application.Tokens.Dtos;

namespace WorkerManagementApi.Application.Tokens.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponseDto> Authenticate(TokenRequestDto request);
        public Task<bool> IsValidUser(string username, string password);
        public Guid? ValidateJwtToken(string token);
    }
}
