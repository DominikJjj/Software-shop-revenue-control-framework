namespace s27400_APBD_Project.ContractPart.DTOs;

public class UpdateLenghtVerificationDTO
{
    public List<SoftwareTimeDTO> IdsWithTime { get; set; }
    public DateTime DateTill { get; set; }
}