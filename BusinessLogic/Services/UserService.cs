using Microsoft.AspNetCore.Identity;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMunicipalityRepository _municipalityRepository;
    private readonly UserManager<IdentityUser<int>> _userManager;

    public UserService(IUserRepository userRepository,UserManager<IdentityUser<int>> userManager, IMunicipalityRepository municipalityRepository)
    {
         _userManager = userManager;
         _userRepository = userRepository;
         _municipalityRepository = municipalityRepository;
    }

   public async Task<IdentityResult> RegisterAsync(RegisterDto model)
    {

    if (model.Role == "Player")
    {
        // Look up the Municipality
var municipality = await _municipalityRepository.findByName(model.MunicipalityName);
if (municipality == null || municipality.Province.ProvinceName != model.ProvinceName)
{
    // Municipality not found or Province name doesn't match
    return IdentityResult.Failed(new IdentityError { Description = "Invalid Municipality or Province name" });
}

// Create the user
Player user = new Player
{
    UserName = model.UserName,
    MunicipalityId = municipality.Id,
    // Don't set the Municipality property here
    Address = model.Address,
    PhoneNumber = model.PhoneNumber,
    Money = 0
};
            var result = await _userRepository.CreateAsync(user, model.Password);
    if (result.Succeeded)
    {
        // Assign the role to the user
        await _userRepository.AddToRoleAsync(user, model.Role);
    }
    
    return result;
    }
    else
    {
        AdministrativeUser user;
        user = new AdministrativeUser { UserName = model.UserName };
        var result = await _userRepository.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            // Assign the role to the user
            await _userRepository.AddToRoleAsync(user, model.Role);
        }

        return result;
    }
}

    public async Task<IdentityUser<int>> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return user;
        }

        return null;
    }

    public async Task<IList<string>> GetUserRolesAsync(IdentityUser<int> user)
    {
        return await _userManager.GetRolesAsync(user);
    }

        public async Task<IEnumerable<UserOutDto>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task UpdateUserAsync(IdentityUser<int> user)
    {
        // Add any business logic here
        await _userRepository.UpdateUserAsync(user);
    }

    public async Task<PlayerOutDto> GetPlayerProfile(int playerId)
    {
    var player = await _userRepository.GetPlayerWithDecks(playerId);

    if (player == null)
    {
        throw new Exception("Player not found");
    }

    var playerOutDto = new PlayerOutDto
    {
        UserName = player.UserName,
        Address = player.Address,
        Money = player.Money,
        PhoneNumber = player.PhoneNumber,
        MunicipalityId = player.MunicipalityId,
        MunicipalityName = player.Municipality.Name,
        ProvinceId = player.Municipality.ProvinceId,
        ProvinceName = player.Municipality.Province.ProvinceName,
        Decks = player.Decks.Select(d => new DeckOutDto 
        { 
            Id = d.Id,
            Name = d.Name, 
            Archetype = d.Archetype, 
            ExtraCards = d.ExtraCards, 
            MainCards = d.MainCards, 
            SideCards = d.SideCards 
        }).ToList()
    };

    return playerOutDto;
}

    }






// public class UserService
// {
//     private readonly UserManager<IdentityUser> _userManager;

//     public UserService(UserManager<IdentityUser> userManager)
//     {
//         _userManager = userManager;
//     }

//     public async Task<IdentityResult> RegisterAsync(RegisterDto model)
//     {
//         IdentityUser user;
//         if (model.Role == "Player")
//         {
//             user = new Player { UserName = model.UserName };
//             // Set additional properties for Player here
//             ((Player)user).Money = 0;
//             ((Player)user).Decks = [];
//             ((Player)user).TournamentPlayers = [];
//             ((Player)user).PhoneNumber = model.PhoneNumber;
//             ((Player)user).Address = model.Address;
//         }
//         else
//         {
//             user = new IdentityUser { UserName = model.UserName };
//         }

//         var result = await _userManager.CreateAsync(user, model.Password);
//         if (result.Succeeded)
//         {
//             // Assign the role to the user
//             await _userManager.AddToRoleAsync(user, model.Role);
//         }

//         return result;
//     }

//     public async Task<IdentityUser> LoginAsync(LoginDto model)
//     {
//         var user = await _userManager.FindByNameAsync(model.UserName);
//         if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
//         {
//             return user;
//         }

//         return null;
//     }

//     public async Task<IList<string>> GetUserRolesAsync(IdentityUser user)
//     {
//         return await _userManager.GetRolesAsync(user);
//     }
// }
