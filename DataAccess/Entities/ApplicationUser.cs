using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser<int>
{
    public string UserName;
    public string Role;
    public string PasswordHash;
}