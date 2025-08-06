namespace s27400_APBD_Project.Entities;

public class Role
{
    public int RoleId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}