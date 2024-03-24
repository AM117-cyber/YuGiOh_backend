using Microsoft.AspNetCore.Identity;

public interface IUserService
{

   Task<IdentityResult> RegisterAsync(RegisterDto model);
   Task<IdentityUser<int>> LoginAsync(LoginDto model);
    Task<IList<string>> GetUserRolesAsync(IdentityUser<int> user);
    Task<IEnumerable<UserOutDto>> GetAllUsersAsync();
    Task UpdateUserAsync(IdentityUser<int> user);
    Task<PlayerOutDto> GetPlayerProfile(int playerId);

}