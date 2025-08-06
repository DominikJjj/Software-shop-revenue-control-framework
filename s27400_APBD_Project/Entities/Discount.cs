namespace s27400_APBD_Project.Entities;

public class Discount
{
    public int DiscountId { get; set; }
    public string Name { get; set; }
    public string Offer { get; set; }
    public int Value { get; set; }
    public DateTime DateStart { get; set; }
    public DateTime DateEnd { get; set; }
    public virtual ICollection<SoftwareSystem> SoftwareSystems { get; set; } = new List<SoftwareSystem>();
}