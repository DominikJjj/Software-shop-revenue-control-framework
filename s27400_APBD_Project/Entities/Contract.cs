namespace s27400_APBD_Project.Entities;

public class Contract
{
    public int ContractId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int StateFK { get; set; }
    public decimal Price { get; set; }
    public virtual State StateNavigation { get; set; }
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual ICollection<ContractSoftware> ContractSoftwares { get; set; } = new List<ContractSoftware>();
}