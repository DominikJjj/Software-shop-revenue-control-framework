using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using s27400_APBD_Project.ContractPart.DTOs;
using s27400_APBD_Project.ContractPart.Services;

namespace s27400_APBD_Project.ContractPart.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContractController : ControllerBase
{
    private readonly IContractService _contractService;

    public ContractController(IContractService contractService)
    {
        _contractService = contractService;
    }


    [HttpPost("contracts")]
    public async Task<IActionResult> AddContract(ContractDTO contractData, CancellationToken token)
    {
        return Ok(await _contractService.CreateContract(contractData, token));
    }
}