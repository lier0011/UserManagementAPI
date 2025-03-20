namespace UserManagementAPI.Models;

public record User
{
    required public string UserName { get; set; }
    public int Age { get; set; }
}
