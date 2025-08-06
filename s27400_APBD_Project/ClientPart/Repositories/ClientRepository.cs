using Microsoft.EntityFrameworkCore;
using s27400_APBD_Project.ClientPart.DTOs;
using s27400_APBD_Project.Entities;

namespace s27400_APBD_Project.ClientPart.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly SoftwareDbContext _context;

    public ClientRepository(SoftwareDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IfGivenClientExists(string pesel, CancellationToken token)
    {
        var res = await _context.Clients
            .FirstOrDefaultAsync(x => x.PESEL == pesel, token);

        if (res == null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IfGivenCompanyExists(string krs, CancellationToken token)
    {
        var res = await _context.Companies
            .FirstOrDefaultAsync(x => x.KRS == krs, token);

        if (res == null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> VerifyClientById(int id, CancellationToken token)
    {
        var res = await _context.Clients
            .FirstOrDefaultAsync(x => x.ClientId == id, token);

        if (res == null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> VerifyCompanyById(int id, CancellationToken token)
    {
        var res = await _context.Companies
            .FirstOrDefaultAsync(x => x.CompanyId == id, token);

        if (res == null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> AddClient(ClientAddDTO newClient, CancellationToken token)
    {
        Client toAdd = new Client()
        {
            Name = newClient.Name,
            Surname = newClient.Surname,
            Email = newClient.Email,
            PhoneNumber = newClient.PhoneNumber,
            PESEL = newClient.PESEL
        };

        await _context.Clients.AddAsync(toAdd, token);
        await _context.SaveChangesAsync(token);
        return true;
    }

    public async Task<bool> AddCompany(CompanyAddDTO newCompany, CancellationToken token)
    {
        Company toAdd = new Company()
        {
            Name = newCompany.Name,
            Address = newCompany.Address,
            Email = newCompany.Email,
            PhoneNumber = newCompany.PhoneNumber,
            KRS = newCompany.KRS
        };

        await _context.Companies.AddAsync(toAdd, token);
        await _context.SaveChangesAsync(token);

        return true;
    }

    public async Task<bool> SoftDeleteClient(int clientId, CancellationToken token)
    {
        var res = await _context.Clients
            .FirstAsync(x => x.ClientId == clientId, token);

        res.IsDeleted = true;

        await _context.SaveChangesAsync(token);

        return true;
    }

    public async Task<bool> UpdateClientInfo(int clientId, ClientPutDTO updatedClient, CancellationToken token)
    {
        var res = await _context.Clients
            .FirstAsync(x => x.ClientId == clientId, token);

        if (updatedClient.Name != "string")
        {
            res.Name = updatedClient.Name;
        }

        if (updatedClient.Surname != "string")
        {
            res.Surname = updatedClient.Surname;
        }

        if (updatedClient.Email != "user@example.com")
        {
            res.Email = updatedClient.Email;
        }

        if (updatedClient.PhoneNumber != "string")
        {
            res.PhoneNumber = updatedClient.PhoneNumber;
        }

        await _context.SaveChangesAsync(token);
        return true;

    }

    public async Task<bool> UpdateCompanyInfo(int companyId, CompanyPutDTO updatedCompany, CancellationToken token)
    {
        var res = await _context.Companies
            .FirstAsync(x => x.CompanyId == companyId, token);
        
        if (updatedCompany.Name != "string")
        {
            res.Name = updatedCompany.Name;
        }

        if (updatedCompany.Address != "string")
        {
            res.Address = updatedCompany.Address;
        }

        if (updatedCompany.Email != "user@example.com")
        {
            res.Email = updatedCompany.Email;
        }

        if (updatedCompany.PhoneNumber != "string")
        {
            res.PhoneNumber = updatedCompany.PhoneNumber;
        }

        await _context.SaveChangesAsync(token);
        return true;

    }

    public bool containsOnlyNumbers(string toTest)
    {
        foreach (char c in toTest)
        {
            if (c < '0' || c > '9')
            {
                return false;
            }
        }

        return true;
    }

    public async Task<bool> ClientAlreadyDeleted(int clientId, CancellationToken token)
    {
        var res = await _context.Clients
            .FirstOrDefaultAsync(x => x.ClientId == clientId, token);

        if (res.IsDeleted)
        {
            return false;
        }

        return true;
    }
    
    

}