using System.Text;
using MessageAppBack.Data;
using MessagerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MessageApp.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;

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
        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage(int SenderId, int ReceiverId, string Content)
        {

            // Get or create a conversation based on the sender and receiver IDs
            var conversation = await _context.Conversations.FirstOrDefaultAsync(c =>
                (c.User1Id == SenderId && c.User2Id == ReceiverId) ||
                (c.User1Id == ReceiverId && c.User2Id == SenderId));

            if (conversation == null)
            {
                // Create a new conversation
                conversation = new Conversation
                {
                    User1Id = SenderId,
                    User2Id = ReceiverId
                };
                _context.Conversations.Add(conversation);
                await _context.SaveChangesAsync();
            }

            // Create and save the message in the database
            var message = new MessageModel
            {
                SenderId = SenderId,
                ReceiverId = ReceiverId,
                Content = Content,
                SentAt = System.DateTime.UtcNow,
                ConversationId = conversation.ConversationId
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok("Message sent successfully.");
        }
    }
}







  