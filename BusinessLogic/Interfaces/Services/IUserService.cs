using Microsoft.AspNetCore.Identity;

public interface IUserService
{

   Task<IdentityResult> RegisterAsync(RegisterDto model);
   Task<IdentityUser<int>> LoginAsync(LoginDto model);
    Task<IList<string>> GetUserRolesAsync(IdentityUser<int> user);
    Task<IEnumerable<UserOutDto>> GetAllUsersAsync();
    Task<PlayerOutDto> GetPlayerProfile(int playerId);
    Task<IEnumerable<PlayerDeckCountDto>> GetPlayersDeckCount();
    Task<(IEnumerable<string>, int)> MostPopularProvinceForArchetype(string archetype);
    Task<(MunicipalityOutDto, int)> MostPopularMunicipalityForArchetype(string archetype);
    Task<PlayerOutDto> UpdatePlayer(PlayerInDto user);
    Task<AdminOutDto> UpdateAdmin(AdminInDto admin);
    Task<bool> findSuperAdmin();
}