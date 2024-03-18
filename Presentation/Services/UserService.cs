using Microsoft.AspNetCore.Identity;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly MunicipalityRepository _municipalityRepository;
    private readonly UserManager<IdentityUser<int>> _userManager;

    public UserService(UserRepository userRepository,UserManager<IdentityUser<int>> userManager, MunicipalityRepository municipalityRepository)
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
        Player user;
        if (model.Address == null || model.PhoneNumber == null)
        {
            ////////////Review this later
            return IdentityResult.Failed(new IdentityError { Description = "Invalid Address or PhoneNumber" });
        }
            user = new Player
            {
                UserName = model.UserName,
                MunicipalityId = municipality.Id,
                Municipality = municipality,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,         // Set additional properties for Player here
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
        IdentityUser<int> user;
        user = new IdentityUser<int> { UserName = model.UserName };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            // Assign the role to the user
            await _userManager.AddToRoleAsync(user, model.Role);
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

        public async Task<IEnumerable<IdentityUser<int>>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task UpdateUserAsync(IdentityUser<int> user)
    {
        // Add any business logic here
        await _userRepository.UpdateUserAsync(user);
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
