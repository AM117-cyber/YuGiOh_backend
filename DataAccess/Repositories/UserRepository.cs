using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserRepository: IUserRepository
{
    private readonly UserManager<IdentityUser<int>> _userManager;

    public UserRepository(UserManager<IdentityUser<int>> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateAsync(IdentityUser<int> user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task AddToRoleAsync(IdentityUser<int> user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

public async Task<IEnumerable<UserOutDto>> GetAllUsersAsync()
{
    var users = await _userManager.Users.ToListAsync();
    var userDtos = new List<UserOutDto>();

    foreach (var user in users)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var userDto = new UserOutDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Role = roles.FirstOrDefault(), // Assumes each user has at most one role
            // Add additional properties here
        };

        if (user is Player player)
        {
            userDto.Address = player.Address;
            userDto.PhoneNumber = player.PhoneNumber;
            userDto.Money = player.Money;
            userDto.MunicipalityId = player.MunicipalityId;
        }

        userDtos.Add(userDto);
    }

    return userDtos;
}
    public async Task<Player> findByIdWithDeck(int id)
    {       
            // Find the Player with the given Id and include their Decks
    var user = await _userManager.Users
        .OfType<Player>()
        .Include(p => p.Decks)
        .SingleOrDefaultAsync(u => u.Id == id);

    if (user == null)
    {
        throw new Exception("User not found");
    }

    var roles = await _userManager.GetRolesAsync(user);

    if (!roles.Contains("Player"))
    {
        throw new Exception("User is not a Player");
    }

    return user;

    }

    public async Task<Player> GetPlayerWithDecks(int playerId)
{
    var player = await _userManager.Users
        .OfType<Player>()
        .Include(p => p.Decks)
        .Include(p => p.Municipality)
        .ThenInclude(m => m.Province)
        .SingleOrDefaultAsync(p => p.Id == playerId);

    if (player == null)
    {
        throw new Exception("Player not found");
    }
    return player;
}


public async Task<IEnumerable<PlayerDeckCountDto>> GetPlayersDeckCount()
{
    var players = await _userManager.Users
        .OfType<Player>() // Only get Player users
        .Include(p => p.Decks) // Include the Decks
        .Select(p => new PlayerDeckCountDto // Select into a DTO
        {
            PlayerId = p.Id,
            UserName = p.UserName,
            DeckCount = p.Decks.Count
        })
        .OrderByDescending(dto => dto.DeckCount) // Order by deck count descending
        .ToListAsync();

    return players;
}

    //     public async Task<IEnumerable<IdentityUser<int>>> GetAllUsersAsync()
    // {
    //     return await _userManager.Users.ToListAsync();
    // }

    public async Task<IdentityResult> UpdateUserAsync(IdentityUser<int> user)
    {
        return await _userManager.UpdateAsync(user);
    }
}

