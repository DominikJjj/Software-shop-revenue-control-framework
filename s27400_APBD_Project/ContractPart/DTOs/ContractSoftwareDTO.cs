using System.ComponentModel.DataAnnotations;

namespace s27400_APBD_Project.ContractPart.DTOs;

public class ContractSoftwareDTO
{
    [Required]
    public int SoftwareSystemFK { get; set; }
    [Required]
    [Range(1,4)]
    public int UpdateTime { get; set; }
}