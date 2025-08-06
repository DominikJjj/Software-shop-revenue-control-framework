namespace s27400_APBD_Project.Entities;

public class SoftwareSystem
{
    public int SoftwareId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public decimal Price { get; set; }
    public int CategoryFK { get; set; }
    public virtual SoftwareCategory SoftwareCategoryNavigation { get; set; }
    public virtual ICollection<ContractSoftware> ContractSoftwares { get; set; } = new List<ContractSoftware>();
    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}