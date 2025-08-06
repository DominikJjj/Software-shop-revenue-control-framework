using s27400_APBD_Project.ClientPart.DTOs;

namespace s27400_APBD_Project.ClientPart.Services;

public interface IClientService
{
    public Task<bool> AddNewClient(ClientAddDTO newClient, CancellationToken token);
    public Task<bool> AddNewCompany(CompanyAddDTO newCompany, CancellationToken token);
    public Task<bool> SoftDeleteClient(int clientId, CancellationToken token);
    public Task<bool> UpdateClient(int clientId, ClientPutDTO updatedClient, CancellationToken token);
    public Task<bool> UpdateCompany(int companyId, CompanyPutDTO updatedCompany, CancellationToken token);
}