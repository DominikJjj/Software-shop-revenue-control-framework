namespace s27400_APBD_Project.Entities;

public class User
{
    public int UserId { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string RefreshToken { get; set; }
    public DateTime DueDateRefreshToken { get; set; }
    public int RoleFK { get; set; }
    public Role RoleNavigation { get; set; }
}