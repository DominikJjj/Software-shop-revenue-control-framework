using System.ComponentModel.DataAnnotations;
using s27400_APBD_Project.ClientPart.DTOs;
using s27400_APBD_Project.ClientPart.Repositories;

namespace s27400_APBD_Project.ClientPart.Services;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;

    public ClientService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<bool> AddNewClient(ClientAddDTO newClient, CancellationToken token)
    {
        bool verifyPhone = _clientRepository.containsOnlyNumbers(newClient.PhoneNumber);

        if (verifyPhone == false)
        {
            throw new Exception("400 Number telefonu może zawierać tylko liczby");
        }

        bool verifyPesel = _clientRepository.containsOnlyNumbers(newClient.PESEL);

        if (verifyPesel == false)
        {
            throw new Exception("400 PESEL może zawierać tylko liczby");
        }
        
        bool verify = await _clientRepository.IfGivenClientExists(newClient.PESEL, token);

        if (verify == true)
        {
            throw new Exception($"400 Klient z PESEL: {newClient.PESEL} juz istnieje w systemie");
        }

        await _clientRepository.AddClient(newClient, token);
        return true;
    }

    public async Task<bool> AddNewCompany(CompanyAddDTO newCompany, CancellationToken token)
    {
        bool verifyPhone = _clientRepository.containsOnlyNumbers(newCompany.PhoneNumber);
        
        if (verifyPhone == false)
        {
            throw new Exception("400 Number telefonu może zawierać tylko liczby");
        }

        bool verifyKRS = _clientRepository.containsOnlyNumbers(newCompany.KRS);

        if (verifyKRS == false)
        {
            throw new Exception("400 KRS może zawierać tylko liczby");
        }
        
        bool verify = await _clientRepository.IfGivenCompanyExists(newCompany.KRS, token);

        if (verify == true)
        {
            throw new Exception($"400 Firma z KRS: {newCompany.KRS} juz istnieje w systemie");
        }

        await _clientRepository.AddCompany(newCompany, token);
        return true;
    }

    public async Task<bool> SoftDeleteClient(int clientId, CancellationToken token)
    {
        bool verify = await _clientRepository.VerifyClientById(clientId, token);

        if (verify == false)
        {
            throw new Exception($"400 Klient o id: {clientId} nie jest wprowadzony w systemie");
        }

        bool verifyDelete = await _clientRepository.ClientAlreadyDeleted(clientId, token);

        if (verifyDelete == false)
        {
            throw new Exception($"400 Klient o id: {clientId} został już usnięty wcześniej");
        }
        
        await _clientRepository.SoftDeleteClient(clientId, token);
        return true;
    }

    public async Task<bool> UpdateClient(int clientId, ClientPutDTO updatedClient, CancellationToken token)
    {
        bool verifyPhone = _clientRepository.containsOnlyNumbers(updatedClient.PhoneNumber);
        
        if (verifyPhone == false)
        {
            throw new Exception("400 Number telefonu może zawierać tylko liczby");
        }
        
        bool verify = await _clientRepository.VerifyClientById(clientId, token);

        if (verify == false)
        {
            throw new Exception($"400 Klient o id: {clientId} nie jest wprowadzony w systemie");
        }

        await _clientRepository.UpdateClientInfo(clientId, updatedClient, token);
        return true;
    }

    public async Task<bool> UpdateCompany(int companyId, CompanyPutDTO updatedCompany, CancellationToken token)
    {
        bool verifyPhone = _clientRepository.containsOnlyNumbers(updatedCompany.PhoneNumber);
        
        if (verifyPhone == false)
        {
            throw new Exception("400 Number telefonu może zawierać tylko liczby");
        }
        
        bool verify = await _clientRepository.VerifyCompanyById(companyId, token);

        if (verify == false)
        {
            throw new Exception($"400 Firma o id: {companyId} nie jest wprowadzony w systemie");
        }

        await _clientRepository.UpdateCompanyInfo(companyId, updatedCompany, token);
        return true;
    }
}