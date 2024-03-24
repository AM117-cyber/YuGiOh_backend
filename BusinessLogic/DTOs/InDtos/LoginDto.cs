public class LoginDto
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public LoginDto(){}
    public LoginDto (string UserName, string Password){
        this.UserName = UserName;
        this.Password = Password;
    }

}