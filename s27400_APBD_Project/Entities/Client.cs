namespace s27400_APBD_Project.Entities;

public class Client
{
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string PESEL { get; set; }
    public bool IsDeleted { get; set; }
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}