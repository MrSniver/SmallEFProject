using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WorkerManagementApi.Application.ApplicationUsers.Repositories;
using WorkerManagementApi.Application.Common.Interfaces;
using WorkerManagementApi.Application.Common.Services;
using WorkerManagementApi.Application.Tokens.Dtos;
using WorkerManagementApi.Application.Tokens.Interfaces;
using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Application.Tokens.Services
{
    public class TokenService: AppService, ITokenService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenDto _tokenDto;
        private readonly HttpContext _context;
        private readonly IApplicationUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IOpenIddictTokenManager _tokenManager;

        public TokenService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IOptions<TokenDto> tokenOptions, 
            IHttpContextAccessor httpContextAccessor, IApplicationUserRepository userRepository, 
            IConfiguration configuration,
            IUnitOfWork unitOfWork, IOpenIddictTokenManager tokenManager): base(unitOfWork)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenDto = tokenOptions.Value ?? throw new ArgumentNullException(nameof(tokenOptions));
            _context = httpContextAccessor.HttpContext;
            _userRepository = userRepository;
            _configuration = configuration;
            _tokenManager = tokenManager;
        }

        public async Task<bool> IsValidUser(string username, string password)
        {
            ApplicationUser user = UnitOfWork.ApplicationUserRepository.GetWhereAsync(x => x.UserName == username).Result.FirstOrDefault();

            if (user == null)
                return false;

            SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, password, true, false);

            return signInResult.Succeeded;
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {

            string role = (await _userManager.GetRolesAsync(user))[0];
            byte[] secret = Encoding.ASCII.GetBytes(_tokenDto.Secret);

            JwtSecurityTokenHandler handler= new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenDto.Issuer,
                Audience = _tokenDto.Audience,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserName", $"{user.UserName}"),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = handler.CreateToken(descriptor);
            return handler.WriteToken(token);
        }

        public Guid? ValidateJwtToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenDto.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "UserId").Value);

                return userId;
            }
            catch (Exception ex) 
            {
                var message = ex.Message;
                return null;
            }
        }

        public async Task<TokenResponseDto> Authenticate(TokenRequestDto request)
        {
            try
            {
                if (!IsValidUser(request.Username, request.Password).Result) return null;
                var user = UnitOfWork.ApplicationUserRepository.GetWhereAsync(x => x.UserName == request.Username).Result.FirstOrDefault();
                if (user == null) return null;

                UnitOfWork.BeginTransaction();

                var role = (await _userManager.GetRolesAsync(user))[0];
                var jwtToken = await GenerateJwtToken(user);
                await UnitOfWork.ApplicationUserRepository.UpdateAsync(user);

                UnitOfWork.SaveChanges();
                UnitOfWork.CommitTransaction();

                return new TokenResponseDto(user, role, jwtToken);
            }
            catch (Exception ex)
            {
                var msg = ex;
                UnitOfWork.RollbackTransaction();
                return null;
            }
                
        }
    }
}
