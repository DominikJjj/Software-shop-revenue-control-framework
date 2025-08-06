using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using s27400_APBD_Project.SecurityPart.DTOs;
using s27400_APBD_Project.SecurityPart.Services;

namespace s27400_APBD_Project.SecurityPart.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecurityController : ControllerBase
{
    private readonly ISecurityService _securityService;

    public SecurityController(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(UserDTO user, CancellationToken token)
    {
        return Ok("Id nowego użytkownika: " + await _securityService.Register(user, token));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginDTO login, CancellationToken token)
    {
        return Ok(await _securityService.Login(login, token));
    }

    [Authorize(AuthenticationSchemes = "IgnoreExpirationScheme")]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshDTO refresh, CancellationToken token)
    {
        return Ok(await _securityService.RefreshToken(refresh, token));
    }
}