namespace s27400_APBD_Project.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public int? ClientFK { get; set; }
    public int? CompanyFK { get; set; }
    public int ContractFK { get; set; }
    public decimal ValuePaid { get; set; }
    public virtual Contract ContractNavigation { get; set; }
    public virtual Company? CompanyNavigation { get; set; }
    public virtual Client? ClientNavigation { get; set; }
}