using SharedLibrary.Middleware;
using System.Security.Claims;
using System.Text;
using SharedLibrary.UserModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace SharedLibrary.Auth
{
    public class Auth : IAuth
    {
        private readonly IConfiguration configuration;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IBlackListService blacklistService;

        public Auth(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IBlackListService blacklistService)
        {
            this.configuration = configuration;
            this.httpContextAccessor = httpContextAccessor;
            this.blacklistService = blacklistService;
        }

        public int? GetCurrentUserId()
        {
            var claimsIdentity = httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
            int id = int.Parse(claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            return id;
        }


        public List<string> GetCurrentUserRoles()
        {
            var claimsIdentity = httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity;
            return claimsIdentity?.FindAll(ClaimTypes.Role).Select(claim => claim.Value).ToList() ?? new List<string>();
        }

        public string GenerateJwtToken<T>(T user)
        {
            var userRoles = new List<string>();

            userRoles.Add("USER");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, (user as UserModel)?.Id.ToString() ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public void Logout(string token)
        {
            blacklistService.AddTokenToBlacklist(token);
        }
    }
}
