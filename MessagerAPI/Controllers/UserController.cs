using System.Text;
using MessageAppBack.Data;
using MessagerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MessageApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MessagerDbContext _context;

        public UserController(MessagerDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(Userr userModel)
        {
            if (ModelState.IsValid)
            {
                // Check if the user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == userModel.Username || u.Email == userModel.Email);

                if (existingUser != null)
                {
                    return Conflict("Username or email already exists.");
                }

                // Add the user to the database
                _context.Users.Add(userModel);
                await _context.SaveChangesAsync();

                return Ok("User registered successfully.");
            }

            return BadRequest("Invalid user data.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Userr userModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user from the database based on the provided username/email
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u =>
                        u.Username == userModel.Username || u.Email == userModel.Email);

                if (existingUser == null)
                {
                    return Unauthorized("Invalid username or email.");
                }

                // Perform user authentication (e.g., verify password)
                // You can customize the authentication logic according to your requirements

                // If authentication succeeds, generate and return the JWT token
                var issuer = HttpContext.Request.Host.Value;
                var audience = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my custom Secret key for authentication"));

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new[]
                    {
                        new System.Security.Claims.Claim("Id", existingUser.UserId.ToString()),
                        new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, existingUser.Username),
                        new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, existingUser.Email),
                        new System.Security.Claims.Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, System.Guid.NewGuid().ToString())
                    }),
                    Expires = System.DateTime.UtcNow.AddDays(7),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
                };

                var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var stringToken = tokenHandler.WriteToken(token);

                return Ok(stringToken);
            }

            return BadRequest("Invalid user data.");
        }
    }
}