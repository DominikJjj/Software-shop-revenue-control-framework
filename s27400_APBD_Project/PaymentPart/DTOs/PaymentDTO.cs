using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace s27400_APBD_Project.PaymentPart.DTOs;

public class PaymentDTO
{
    [Required]
    public int ContractId { get; set; }
    [Precision(10,2)]
    [Required]
    public decimal PaymentValue { get; set; }
}