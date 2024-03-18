using Microsoft.AspNetCore.Identity;

public class RoleService
{
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public RoleService(RoleManager<IdentityRole<int>> roleManager)
    {
        _roleManager = roleManager;
    }

    public async Task CreateRoles()
    {
        if (!await _roleManager.RoleExistsAsync("Administrator"))
        {
            await _roleManager.CreateAsync(new IdentityRole<int>{ Name = "Administrator" });
        }
        if (!await _roleManager.RoleExistsAsync("Player"))
        {
            await _roleManager.CreateAsync(new IdentityRole<int>{ Name = "Player" });
        }
        if (!await _roleManager.RoleExistsAsync("SuperAdministrator"))
        {
            await _roleManager.CreateAsync(new IdentityRole<int>{ Name = "SuperAdministrator" });
        }
    }
}
