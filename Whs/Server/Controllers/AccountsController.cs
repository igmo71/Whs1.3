using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Whs.Shared.Models.Accounts;
using Whs.Shared.Utils;

namespace Whs.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfigurationSection _jwtSettings;

        public AccountsController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _jwtSettings = configuration.GetSection("JWT");

        }

        [HttpPost("registration")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration == null || !ModelState.IsValid)
                return BadRequest();

            var user = new ApplicationUser
            {
                FullName = userForRegistration.FullName,
                UserName = userForRegistration.Email,
                Email = userForRegistration.Email,
                WarehouseId = userForRegistration.StorageKey
            };

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegistrationResponseDto { Errors = errors });
            }

            await _userManager.AddToRoleAsync(user, "User");

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserForAuthenticationDto userForAuthentication)
        {
            if (userForAuthentication == null || !ModelState.IsValid)
                return BadRequest();

            ApplicationUser user;

            if (!string.IsNullOrWhiteSpace(userForAuthentication.Barcode))
            {
                var id = GuidConvert.FromNumStr(userForAuthentication.Barcode);
                user = await _userManager.FindByIdAsync(id);
                if (user == null)
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Ошибка авторизации (Пользователь не найден)" });
            }
            else if (!(string.IsNullOrWhiteSpace(userForAuthentication.Email) || string.IsNullOrWhiteSpace(userForAuthentication.Password)))
            {
                user = await _userManager.FindByNameAsync(userForAuthentication.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, userForAuthentication.Password))
                    return Unauthorized(new AuthResponseDto { ErrorMessage = "Ошибка авторизации (Имя и Пароль не совпадают)" });
            }
            else
            {
                return Unauthorized(new AuthResponseDto { ErrorMessage = "Ошибка авторизации (Имя и Пароль обязательны)" });
            }

            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaimsAsync(user);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            var result = new AuthResponseDto { IsAuthSuccessful = true, Token = token };
            return Ok(result);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("securityKey").Value);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            byte[] surnameData = Encoding.UTF8.GetBytes(user.FullName);
            string surnameBase64 = Convert.ToBase64String(surnameData);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Surname, surnameBase64),
                new Claim(ClaimTypes.UserData, user.Id),
                new Claim(ClaimTypes.GroupSid, string.IsNullOrEmpty(user.WarehouseId) ? Guid.Empty.ToString() : user.WarehouseId)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings.GetSection("validIssuer").Value,
                audience: _jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings.GetSection("expiryInMinutes").Value)),
                signingCredentials: signingCredentials);

            return tokenOptions;
        }
    }
}
