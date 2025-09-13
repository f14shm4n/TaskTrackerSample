using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskTracker.API.Application.Jwt;

namespace TaskTracker.API.Controllers
{
    [AllowAnonymous]
    [Route("[controller]")]
    public class JwtGeneratorController : ControllerBase
    {
        private readonly AppJwtOptions _options;

        public JwtGeneratorController(IOptions<AppJwtOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Создает новый JWT токен. Вы можете вводить любые данные.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        [HttpGet("generate")]
        public string GenerateJwtToken(string userId, string username, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_options.ExpiryDays));

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
