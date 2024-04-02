using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public interface IUserRepository
{
    Task<IdentityResult> CreateAsync(IdentityUser<int> user, string password);
    Task AddToRoleAsync(IdentityUser<int> user, string role);
    Task<IEnumerable<UserOutDto>> GetAllUsersAsync();
    Task<IdentityResult> UpdateUserAsync(IdentityUser<int> user);
    Task<Player> findByIdWithDeck(int id);
    Task<Player> findPlayerById(int id);
    Task<AdministrativeUser> findAdminById(int id);
    Task<Player> GetPlayerWithDecks(int playerId);
    Task<IEnumerable<Municipality>> GetPlayersMunicipalities(IEnumerable<int> playersIds);
    Task<IEnumerable<PlayerDeckCountDto>> GetPlayersDeckCount();
    Task<(IEnumerable<Municipality>, int)> GetMostPopularMunicipalityForArchetype(string archetype);
    Task<(IEnumerable<string>, int)> GetMostPopularProvinceForArchetype(string archetype);
    Task<Player> findPlayerByName(string userName);
    Task<AdministrativeUser> findAdminUserByName(string userName);
    Task<bool> FindSuperAdmin();
}

