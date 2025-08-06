using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace s27400_APBD_Project.ContractPart.DTOs;

public class ContractDTO
{
    [Required]
    public PaymentAddDTO ClientInfo { get; set; }
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public List<ContractSoftwareDTO> Softwares { get; set; }
}