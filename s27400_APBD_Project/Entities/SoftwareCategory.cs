namespace s27400_APBD_Project.Entities;

public class SoftwareCategory
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<SoftwareSystem> SoftwareSystems { get; set; } = new List<SoftwareSystem>();
}