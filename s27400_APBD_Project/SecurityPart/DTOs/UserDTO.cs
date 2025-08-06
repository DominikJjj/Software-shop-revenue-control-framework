using System.ComponentModel.DataAnnotations;

namespace s27400_APBD_Project.SecurityPart.DTOs;

public class UserDTO
{
    [Required]
    [MaxLength(40)]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    [Range(1,2)]
    public int RoleId { get; set; }
}