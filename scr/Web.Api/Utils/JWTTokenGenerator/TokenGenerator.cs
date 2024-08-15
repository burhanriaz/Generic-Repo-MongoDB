using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Web.Api.Infrastructure.Configurations;
using Web.Domain.Models.Account;
using Web.Entity.Infrastructure.Options;

namespace Web.Api.Utils.JWTTokenGenerator
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IOptions<SecretsSettings> encryptionSettings;
        private readonly IOptions<JwtSettings> jwtSettings;
        private readonly ILogger<TokenGenerator> logger;

        public TokenGenerator(ILogger<TokenGenerator> _logger, IOptions<SecretsSettings> _encryptionSettings, IOptions<JwtSettings> _jwtSettings)
        {
            encryptionSettings = _encryptionSettings;
            jwtSettings = _jwtSettings;
            logger = _logger;
        }

        public string GenerateToken(UserViewModel userVm)
        {
            try
            {
                var utcNow = DateTime.UtcNow;

                using var privateRsa = RSA.Create();
                privateRsa.FromXmlString(encryptionSettings.Value.JwtRsaPrivateKeyXml);
                var privateKey = new RsaSecurityKey(privateRsa) { KeyId = jwtSettings.Value.KeyId };
                var signingCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256)
                {
                    CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
                };

                //var claims = new List<Claim>
                //{
                //    new Claim(JwtRegisteredClaimNames.Sub, userVm.Id.ToString()),
                //    new Claim(JwtRegisteredClaimNames.Email, userVm.Email),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                //};
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,userVm.Id.ToString()),
                    new Claim(ClaimTypes.Role,userVm.Roles.FirstOrDefault().ToString()),
                    new Claim(ClaimTypes.Email,userVm.Email),
            };

                //foreach (var role in userVm.Roles)
                //{
                //    claims.Add(new Claim(ClaimTypes.Role, role.Name.ToString()));
                //}

                var jwt = new JwtSecurityToken(
                    signingCredentials: signingCredentials,
                    claims: claims,
                    notBefore: utcNow,
                    expires: utcNow.AddDays(jwtSettings.Value.LifetimeDays),
                    audience: jwtSettings.Value.Audience,
                    issuer: jwtSettings.Value.Issuer);

                return new JwtSecurityTokenHandler().WriteToken(jwt);
            }
            catch (Exception ex)
            {
                logger.LogError($"Exception occured during Generate Token: {ex.Message}");
                throw;
            }
        }
    }
}
