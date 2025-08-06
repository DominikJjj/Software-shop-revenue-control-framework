using System.ComponentModel.DataAnnotations;

namespace s27400_APBD_Project.SecurityPart.DTOs;

public class RefreshDTO
{
    [Required]
    public string Token { get; set; }
}