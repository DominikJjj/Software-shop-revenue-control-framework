using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using s27400_APBD_Project.ClientPart.DTOs;
using s27400_APBD_Project.ClientPart.Services;

namespace s27400_APBD_Project.ClientPart.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [Authorize]
    [HttpPost("clients")]
    public async Task<IActionResult> AddClient(ClientAddDTO newClient, CancellationToken token)
    {
        await _clientService.AddNewClient(newClient, token);

        return Ok("Nowy klient poprawnie dodany");
    }

    [Authorize]
    [HttpPost("comapnies")]
    public async Task<IActionResult> AddCompany(CompanyAddDTO newCompany, CancellationToken token)
    {
        await _clientService.AddNewCompany(newCompany, token);
        
        return Ok("Nowa firma poprawnie dodana");

    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("client/{clientId}/softDelete")]
    public async Task<IActionResult> SoftDeleteClient(int clientId, CancellationToken token)
    {
        await _clientService.SoftDeleteClient(clientId, token);

        return Ok($"Klient o id: {clientId} został usunięty miękko");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("client/{clientId}/updateData")]
    public async Task<IActionResult> UpdateClient(int clientId, ClientPutDTO updatedClient, CancellationToken token)
    {
        await _clientService.UpdateClient(clientId, updatedClient, token);

        return Ok($"Klient o id: {clientId} został poprawnie zaktualizowany");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("company/{companyId}/updateData")]
    public async Task<IActionResult> UpdateCompany(int companyId, CompanyPutDTO updatedCompany, CancellationToken token)
    {
        await _clientService.UpdateCompany(companyId, updatedCompany, token);
        
        return Ok($"Firma o id: {companyId} została poprawnie zaktulizowania");
    }
    
    
}