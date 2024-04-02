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

    var player = await _userRepository.findPlayerByName(model.UserName);
    if (player != null)
    {
        return IdentityResult.Failed(new IdentityError { Description = "Invalid name" });
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
        var admininUser = await _userRepository.findAdminUserByName(model.UserName);
    if (admininUser != null)
    {
        return IdentityResult.Failed(new IdentityError { Description = "Invalid name" });
    }
        user = new AdministrativeUser { UserName = model.UserName };
        var result = await _userRepository.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            // Assign the role to the user
            await _userRepository.AddToRoleAsync(user, "Administrator");
            if (model.Role == "SuperAdministrator")
            {
                await _userRepository.AddToRoleAsync(user, "SuperAdministrator");
            }
        }

        return result;
    }
}

    public async Task<IdentityUser<int>> LoginAsync(LoginDto model)
    {
        if (model.Role == "Player")
        {
            var user = await _userRepository.findPlayerByName(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return user;
        }

        return null;
        }else
        {
            var user = await _userRepository.findAdminUserByName(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            return user;
        }

        return null;
        }
        
        
    }

    public async Task<IList<string>> GetUserRolesAsync(IdentityUser<int> user)
    {
        return await _userManager.GetRolesAsync(user);
    }

        public async Task<IEnumerable<UserOutDto>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    // public async Task UpdateUserAsync(IdentityUser<int> user)
    // {
    //     // Add any business logic here
    //     await _userRepository.UpdateUserAsync(user);
    // }

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
        Decks = player.Decks.Where(d => d.MyStatus == EntityStatus.visible).Select(d => new DeckOutDto 
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

    public async Task<IEnumerable<PlayerDeckCountDto>> GetPlayersDeckCount()
    {
        return await _userRepository.GetPlayersDeckCount();
    }

    public async Task<(IEnumerable<string>, int)> MostPopularProvinceForArchetype(string archetype)
    {   
        var result = await _userRepository.GetMostPopularProvinceForArchetype(archetype);
        return result;
    }

    public async Task<(IEnumerable<MunicipalityOutDto>, int)> MostPopularMunicipalityForArchetype(string archetype)
    {
        (IEnumerable<Municipality> municipalities, int count) result = await _userRepository.GetMostPopularMunicipalityForArchetype(archetype);
        IEnumerable<MunicipalityOutDto> municipalityOutDtos = result.municipalities.Select(m => new MunicipalityOutDto 
        { 
            Name = m.Name,
            ProvinceName = m.Province.ProvinceName
        }).ToList();
        return (municipalityOutDtos, result.count);
    }

    public async Task<PlayerOutDto> UpdatePlayer(PlayerInDto user)
{
    var player = await _userRepository.findPlayerById(user.Id);

    var municipality = await _municipalityRepository.findByName(user.MunicipalityName);
    if (municipality == null || municipality.Province.ProvinceName != user.ProvinceName)
    {
        throw new ArgumentException("Ese municipio es inválido.");
    }
    // Update the player's attributes
    player.UserName = user.UserName;
    player.Address = user.Address;
    player.PhoneNumber = user.PhoneNumber;
    player.MunicipalityId = municipality.Id;
    player.PasswordHash = _userManager.PasswordHasher.HashPassword(player, user.Password);

    // Save the changes
    var result = await _userRepository.UpdateUserAsync(player);
    if (!result.Succeeded)
    {
        throw new ArgumentException("Error en el nombre");
    }

    var playerOutDto = new PlayerOutDto
    {
        UserName = user.UserName,
        Address = user.Address,
        Money = player.Money,
        PhoneNumber = user.PhoneNumber,
        MunicipalityId = municipality.Id,
        MunicipalityName = municipality.Name,
        ProvinceId = municipality.ProvinceId,
        ProvinceName = municipality.Province.ProvinceName,
    };

    return playerOutDto;
}

    public async Task<AdminOutDto> UpdateAdmin(AdminInDto adminInDto)
    {
        var admin = await _userRepository.findAdminById(adminInDto.id);
        admin.PasswordHash = _userManager.PasswordHasher.HashPassword(admin, adminInDto.Password);
        admin.UserName = adminInDto.Name;
         await _userRepository.UpdateUserAsync(admin);

    // Map the player to PlayerOutDto
    var adminOutDto = new AdminOutDto
    {
        Name = adminInDto.Name,
    };
    return adminOutDto;
    }

    public async Task<bool> findSuperAdmin()
    {
        var result = await _userRepository.FindSuperAdmin();
        
        return result;
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
