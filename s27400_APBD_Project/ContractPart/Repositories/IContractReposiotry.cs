using s27400_APBD_Project.ContractPart.DTOs;
using s27400_APBD_Project.Entities;

namespace s27400_APBD_Project.ContractPart.Repositories;

public interface IContractReposiotry
{
    public bool ValidateDates(DateTime start, DateTime end);
    public bool ValidateList(List<ContractSoftwareDTO> softwareList);
    public Task<bool> ValidateClient(PaymentAddDTO dto, CancellationToken token);
    public Task<bool> ValidateSoftware(List<ContractSoftwareDTO> softwareList, CancellationToken token);
    public Task<bool> ValidateOwnership(ContractDTO contract, CancellationToken token);
    public Task<List<NewFixPriceDTO>> CalculatePriceWithoutAdditionalUpdate(List<ContractSoftwareDTO> softwareList, CancellationToken token);
    public decimal CalculateAdditionalUpdatesPrice(List<ContractSoftwareDTO> softwareList, List<NewFixPriceDTO> fix);
    public Task<decimal> CalculateRegularCustomerDiscount(string type, int id, decimal price, CancellationToken token);
    public Task<int> AddContract(ContractDTO contract, CancellationToken token, decimal price);
    public Task<bool> AddPayemnt(ContractDTO contract, int contractId, CancellationToken token);
    public Task<List<ContractSoftware>> GetValuesToAddContractSoftware(ContractDTO contract, int contractId, CancellationToken token, List<NewFixPriceDTO> fix);
    public Task<bool> AddAllContractSoftware(List<ContractSoftware> all, CancellationToken token);
    public Task<string> AddingPackedInTransaction(ContractDTO contract, CancellationToken token, decimal price, List<NewFixPriceDTO> fix);
}