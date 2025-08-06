using s27400_APBD_Project.ClientPart.DTOs;

namespace s27400_APBD_Project.ClientPart.Repositories;

public interface IClientRepository
{
    public Task<bool> IfGivenClientExists(string pesel, CancellationToken token);
    public Task<bool> IfGivenCompanyExists(string krs, CancellationToken token);
    public Task<bool> VerifyClientById(int id, CancellationToken token);
    public Task<bool> VerifyCompanyById(int id, CancellationToken token);
    public Task<bool> AddClient(ClientAddDTO newClient, CancellationToken token);
    public Task<bool> AddCompany(CompanyAddDTO newCompany, CancellationToken token);
    public Task<bool> SoftDeleteClient(int clientId, CancellationToken token);
    public Task<bool> UpdateClientInfo(int clientId, ClientPutDTO updatedClient, CancellationToken token);
    public Task<bool> UpdateCompanyInfo(int companyId, CompanyPutDTO updatedCompany, CancellationToken token);
    public bool containsOnlyNumbers(string toTest);
    public Task<bool> ClientAlreadyDeleted(int clientId, CancellationToken token);
}