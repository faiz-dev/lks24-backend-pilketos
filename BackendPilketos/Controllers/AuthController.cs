using Backend1.Responses;
using BackendPilketos.Models;
using BackendPilketos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace BackendPilketos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private UserService _userService;
        private IConfiguration _configuration;

        public AuthController(DataContext context, IConfiguration configuration)
        {
            _userService = new UserService(context);
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login(string email, string? token)
        {
            User user = await _userService.Get(email);

            return new LoginResponse
            {
                Token = CreateToken(user),
                RefreshToken = CreateToken(user),
                DisplayName = user.Name,
                Role = user.Role,
                GroupId = user.Group == null ? null : user.Group.Id
            };
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            var tokenkey = _configuration.GetSection("AppSettings:Token").Value;
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenkey));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
