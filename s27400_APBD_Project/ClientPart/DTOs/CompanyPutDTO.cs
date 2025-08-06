using System.ComponentModel.DataAnnotations;

namespace s27400_APBD_Project.ClientPart.DTOs;

public class CompanyPutDTO
{
    [Required]
    [MaxLength(60)]
    public string Name { get; set; }
    [Required]
    [MaxLength(150)]
    public string Address { get; set; }
    [Required]
    [MaxLength(60)]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [MinLength(9)]
    [MaxLength(9)]
    public string PhoneNumber { get; set; }
}