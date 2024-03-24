using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public interface IUserRepository
{
    Task<IdentityResult> CreateAsync(IdentityUser<int> user, string password);
    Task AddToRoleAsync(IdentityUser<int> user, string role);
    Task<IEnumerable<UserOutDto>> GetAllUsersAsync();
    Task<IdentityResult> UpdateUserAsync(IdentityUser<int> user);
    Task<Player> findByIdWithDeck(int id);
    Task<Player> GetPlayerWithDecks(int playerId);
}

