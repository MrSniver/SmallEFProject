using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkerManagementApi.Application.Tokens.Dtos;

namespace WorkerManagementApi.Config
{
    public static class AuthenticationConfig
    {
        /// <summary>
        ///     authentication configuration
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static void SetupAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            TokenDto token = configuration.GetSection("token").Get<TokenDto>();
            byte[] secret = Encoding.ASCII.GetBytes(token.Secret);

            services
                .AddAuthentication(
                    options =>
                    {
                    })
                .AddJwtBearer(
                    options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.SaveToken = true;
                        options.ClaimsIssuer = token.Issuer;
                        options.IncludeErrorDetails = true;
                        options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                ClockSkew = TimeSpan.Zero,
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ValidIssuer = token.Issuer,
                                ValidAudience = token.Audience,
                                LifetimeValidator = TokenLifetimeValidator.Validate,
                                IssuerSigningKey = new SymmetricSecurityKey(secret),
                                RequireSignedTokens = true,
                                RequireExpirationTime = true,
                            };
                    });
        }
        public static class TokenLifetimeValidator
        {
            public static bool Validate(
                DateTime? notBefore,
                DateTime? expires,
                SecurityToken tokenToValidate,
                TokenValidationParameters @param
            )
            {
                return (expires != null && expires > DateTime.Now);
            }
        }
    }
}
