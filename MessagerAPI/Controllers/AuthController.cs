using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MessagerAPI.Models;
using MessagerAPI.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;


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
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
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

            // Retrieve the receiver user by email
            var receiver = await _userManager.FindByEmailAsync(receiverEmail);
            if (receiver == null)
            {
                // Handle the case when the receiver is not found
                return BadRequest("Receiver not found.");
            }


            // Retrieve the conversation from the database based on sender email
            var conversation = await _context.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Messages.Any(m => m.Sender.Email == user.Email && m.Receiver.Email == receiver.Email) ||
            c.Messages.Any(m => m.Sender.Email == receiver.Email && m.Receiver.Email == user.Email));

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
                SenderId=user.Id,
                Receiver = receiver,
                ReceiverId=receiver.Id,
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
            if (user == null)
            {
                // Handle the case when the user is not found
                return BadRequest("User not found.");
            }

            // Retrieve the user's messages from the database
            var messages = _context.Messages
                .Where(m => m.Sender.Email == user.Email || m.Receiver.Email == user.Email)
                .ToList();

            return Ok(messages);
        }

        [HttpGet]
        [Route("conversations")]
        [Authorize]
        public async Task<IActionResult> GetConversationsAsync(string receiverEmail)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Handle the case when the user is not found
                return BadRequest("User not found.");
            }


            // Retrieve the receiver user by email
            var receiver = await _userManager.FindByEmailAsync(receiverEmail);
            if (receiver == null)
            {
                // Handle the case when the receiver is not found
                return BadRequest("Receiver not found.");
            }
            // Retrieve the conversations between the sender and the specified receiver
            var conversations = _context.Conversations
                .Include(c => c.Messages)
                .Where(c =>
                    (c.Messages.Any(m => m.Sender.UserName == user.UserName && m.Receiver.UserName == receiver.UserName)) ||
                    (c.Messages.Any(m => m.Sender.UserName == receiver.UserName && m.Receiver.UserName == user.UserName))
                )
                .ToList();


            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            // Serialize the messages using the options
            var json = JsonSerializer.Serialize(conversations, options);

            return Ok(json);
        }
}}