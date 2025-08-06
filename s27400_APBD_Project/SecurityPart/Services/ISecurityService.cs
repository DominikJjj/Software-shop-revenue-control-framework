using s27400_APBD_Project.SecurityPart.DTOs;

namespace s27400_APBD_Project.SecurityPart.Services;

public interface ISecurityService
{
    public Task<int> Register(UserDTO user, CancellationToken token);
    public Task<string> Login(LoginDTO login, CancellationToken token);
    public Task<string> RefreshToken(RefreshDTO refresh, CancellationToken token);
}