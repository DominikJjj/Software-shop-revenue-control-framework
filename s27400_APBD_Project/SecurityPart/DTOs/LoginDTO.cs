using System.ComponentModel.DataAnnotations;

namespace s27400_APBD_Project.SecurityPart.DTOs;

public class LoginDTO
{
    [Required]
    [MaxLength(40)]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}