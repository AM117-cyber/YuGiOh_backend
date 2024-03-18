using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class UserRepository
{
    private readonly UserManager<IdentityUser<int>> _userManager;

    public UserRepository(UserManager<IdentityUser<int>> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateAsync(Player user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task AddToRoleAsync(Player user, string role)
    {
        await _userManager.AddToRoleAsync(user, role);
    }

        public async Task<IEnumerable<IdentityUser<int>>> GetAllUsersAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<IdentityResult> UpdateUserAsync(IdentityUser<int> user)
    {
        return await _userManager.UpdateAsync(user);
    }
}

