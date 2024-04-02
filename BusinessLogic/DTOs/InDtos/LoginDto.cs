public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role {get; set; }

    public LoginDto(){}
    public LoginDto (string UserName, string Password, string Role){
        this.UserName = UserName;
        this.Password = Password;
        this.Role = Role;
    }

}