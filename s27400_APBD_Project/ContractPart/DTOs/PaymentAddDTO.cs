using System.ComponentModel.DataAnnotations;

namespace s27400_APBD_Project.ContractPart.DTOs;

public class PaymentAddDTO
{
    [Required]
    [RegularExpression("Firma|Klient", ErrorMessage = "Dopuszczalne wartości w tym polu to Klient lub Firma")]
    public string Firma_czy_Klient { get; set; }
    [Required]
    public int ClientId { get; set; }
}