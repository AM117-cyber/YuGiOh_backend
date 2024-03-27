using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IConfiguration _configuration;

    public UserController(IUserService userService, IConfiguration configuration)
    {
        _userService = userService;
        _configuration = configuration;
    }

    private string GenerateJwtToken(IEnumerable<Claim> claims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // Get the secret key from configuration
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var result = await _userService.RegisterAsync(model);
        if (!result.Succeeded) return BadRequest(result.Errors);

        var user = await _userService.LoginAsync(new LoginDto(model.UserName, model.Password));
        var roles = await _userService.GetUserRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
           // new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Role, string.Join(",", roles))
        };

        return CreatedAtAction("Register", new { Token = GenerateJwtToken(claims) });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var user = await _userService.LoginAsync(model);
        if (user == null) return Unauthorized();

        var roles = await _userService.GetUserRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
            // new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Role, string.Join(",", roles))
        };

        return Ok(new { Token = GenerateJwtToken(claims) });
    }

     [HttpGet("findAll")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("getPlayerProfile/{playerId}")]
    public async Task<IActionResult> GetPlayerProfile(int playerId)
    {
        var profile = await _userService.GetPlayerProfile(playerId);
        return Ok(profile);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser(IdentityUser<int> user)
    {
        await _userService.UpdateUserAsync(user);
        return NoContent();
    }

    [HttpGet("playersDeckCount")]
    public Task<IEnumerable<PlayerDeckCountDto>> GetPlayersDeckCount()
    {
        return _userService.GetPlayersDeckCount();
    }
}







// public class UserController : ControllerBase
// {
//     private readonly UserManager<IdentityUser> _userManager;
//     private readonly IConfiguration _configuration;

//     public UserController(UserManager<IdentityUser> userManager, IConfiguration configuration)
//     {
//         _userManager = userManager;
//         _configuration = configuration;
//     }

//     private string GenerateJwtToken(IEnumerable<Claim> claims)
//     {
//         var tokenHandler = new JwtSecurityTokenHandler();
//         var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); // Get the secret key from configuration
//         var tokenDescriptor = new SecurityTokenDescriptor
//         {
//             Subject = new ClaimsIdentity(claims),
//             SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
//                 SecurityAlgorithms.HmacSha256Signature)
//         };
//         var token = tokenHandler.CreateToken(tokenDescriptor);
//         return tokenHandler.WriteToken(token);
//     }

//   public async Task<IActionResult> Register([FromForm] RegisterDto model)
// {
//     IdentityUser user;
//     if (model.Role == "Player")
//     {
//         user = new Player { UserName = model.UserName };
//         // Set additional properties for Player here
//         ((Player)user).Money = 0;
//         ((Player)user).Decks = [];
//         ((Player)user).TournamentPlayers = [];
//         ((Player)user).PhoneNumber = model.PhoneNumber;
//         ((Player)user).Address = model.Address;
//     }
//     else
//     {
//         user = new IdentityUser { UserName = model.UserName };
//     }

//     var result = await _userManager.CreateAsync(user, model.Password);
//     if (!result.Succeeded) return BadRequest(result.Errors);

//     // Assign the role to the user
//     await _userManager.AddToRoleAsync(user, model.Role);

//     var claims = new List<Claim>
//     {
//         new(ClaimTypes.Name, user.UserName),
//         new(ClaimTypes.NameIdentifier, user.Id),
//         new(ClaimTypes.Role, model.Role)
//     };

//     return CreatedAtAction("Register", new { Token = GenerateJwtToken(claims) });
// }

//      public async Task<IActionResult> Login([FromForm] LoginDto model)
//     {
//         var user = await _userManager.FindByNameAsync(model.UserName);
//         if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
//         {
//             var claims = new List<Claim>
//             {
//                 new(ClaimTypes.Name, user.UserName),
//                 new(ClaimTypes.NameIdentifier, user.Id)
//             };

//             // Get the user's roles
//             var roles = await _userManager.GetRolesAsync(user);

//             // Add each role as a claim
//             claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));


//             return Ok(new { Token = GenerateJwtToken(claims) });
//         }

//         return Unauthorized();
//     }
// }
