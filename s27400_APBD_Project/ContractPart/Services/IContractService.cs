using s27400_APBD_Project.ContractPart.DTOs;

namespace s27400_APBD_Project.ContractPart.Services;

public interface IContractService
{
    public Task<string> CreateContract(ContractDTO contractData, CancellationToken token);
}