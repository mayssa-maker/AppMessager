using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MessagerAPI.Models;
using MessagerAPI.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace MessagerAPI.Controllers{
[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly MessagerDbContext _context;
    private readonly TokenService _tokenService;

    public AuthController(UserManager<IdentityUser> userManager, MessagerDbContext context, TokenService tokenService)

    {    
        _userManager = userManager;
        _context = context;
        _tokenService = tokenService;
    }
    
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegistrationRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _userManager.CreateAsync(
            new IdentityUser { UserName = request.Username, Email = request.Email},
            request.Password
        );
        if (result.Succeeded)
        {
            request.Password = "";
            return CreatedAtAction(nameof(Register), new {email = request.Email}, request);
        }
        foreach (var error in result.Errors) {
            ModelState.AddModelError(error.Code, error.Description);
        }
        return BadRequest(ModelState);
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

       var managedUser = await _userManager.FindByEmailAsync(request.Email);
        if (managedUser == null)
        {
            return BadRequest("Bad credentials");
        }
        var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password);
        if (!isPasswordValid)
        {
            return BadRequest("Bad credentials");
        }
        var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);
        if (userInDb is null)
            return Unauthorized();
        var accessToken = _tokenService.CreateToken(userInDb);
        await _context.SaveChangesAsync();
        return Ok(new AuthResponse
        {
            Username = userInDb.UserName,
            Email = userInDb.Email,
            Token = accessToken,
        });
    }
        [HttpPost]
        [Route("sendMessage")]
        [Authorize]
        public async Task<IActionResult> SendMessage(string content, string receiverEmail)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Handle the case when the user is not found
                return BadRequest("User not found.");
            }

            var userEmail = await _userManager.GetEmailAsync(user);
            if (string.IsNullOrEmpty(userEmail))
            {
                // Handle the case when the email is missing or empty
                return BadRequest("Email is missing or invalid.");
            }
           

            // Retrieve the conversation from the database based on sender email
            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Messages.Any(m => m.SenderEmail == userEmail || m.ReceiverEmail == userEmail));

            if (conversation == null)
            {
                // Create a new conversation if it doesn't exist
                conversation = new Conversation
                {
                    Messages = new List<MessageModel>()
                };
                _context.Conversations.Add(conversation);
            }

            // Create and save the message in the database
            var message = new MessageModel
            {
                Sender = user,
                Receiver = await _userManager.FindByEmailAsync(receiverEmail),
                Content = content,
                SentAt = DateTime.UtcNow
            };

            conversation.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok("Message sent successfully.");
        }

        [HttpGet]
        [Route("messages")]
        [Authorize]
        public async Task<IActionResult> GetUserMessagesAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var userEmail = user.Email;


            // Retrieve the user's messages from the database
            var messages = _context.Messages
                .Where(m => m.SenderEmail == userEmail|| m.ReceiverEmail== userEmail)
                .ToList();

            return Ok(messages);
        }
}}