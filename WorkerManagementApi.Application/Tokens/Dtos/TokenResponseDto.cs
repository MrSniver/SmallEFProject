using WorkerManagementApi.Domain.ApplicationUsers.Entites;

namespace WorkerManagementApi.Application.Tokens.Dtos
{
    public class TokenResponseDto
    {
        public TokenResponseDto(ApplicationUser user, string role, string token)
        {
            Id = user.Id;
            Username = user.UserName;
            Email = user.Email;
            Token = token;
            Role = role;
            Expire = (DateTime.UtcNow.AddMinutes(10)).ToString();
        }

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
        public string Expire { get; set; }
    }
}
