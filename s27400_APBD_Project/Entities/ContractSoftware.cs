namespace s27400_APBD_Project.Entities;

public class ContractSoftware
{
    public int ContractSoftwareId { get; set; }
    public int ContractFK { get; set; }
    public int SoftwareSystemFK { get; set; }
    public int UpdateTime { get; set; }
    public string Version { get; set; }
    public decimal PriceInContract { get; set; }
    public Contract ContractNavigation { get; set; }
    public SoftwareSystem SoftwareSystemNavigation { get; set; }
}