namespace s27400_APBD_Project.Entities;

public class State
{
    public int StateId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}