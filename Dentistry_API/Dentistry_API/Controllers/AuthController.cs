using Dentistry_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Dentistry_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DentistryContext _context;

        public AuthController(DentistryContext context)
        {
            _context = context;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GetToken([FromBody] LoginCredentials credentials)
        {
            // Проверка учетных данных (здесь может быть ваша логика аутентификации)
            var isValidUser = await IsValidUser(credentials);

            if (isValidUser)
            {
                var token = GenerateToken(credentials.Email);
                return Ok(new { token });
            }

            return Unauthorized();
        }

        private async Task<bool> IsValidUser(LoginCredentials model)
        {
            var patient = await _context.Patients
                .FirstOrDefaultAsync(u => u.EmailPatient == model.Email && u.PasswordPatient == model.Password);

            var employee = await _context.Employees
                .FirstOrDefaultAsync(u => u.EmailEmployee == model.Email && u.PasswordEmployee == model.Password);

            return patient != null || employee != null;
        }

        private string GenerateToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("secret-key-dantes"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7015",
                audience: "https://localhost:7136",
                claims: new[] { new Claim(ClaimTypes.Name, username) },
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: credentials
            );

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(token);

            var json = JsonSerializer.Serialize(new { token = tokenString }); // Сериализуем в формат JSON

            return json;
        }


    }
}
