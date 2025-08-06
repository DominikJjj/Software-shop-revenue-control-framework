namespace s27400_APBD_Project.Entities;

public class Company
{
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string KRS { get; set; }
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}