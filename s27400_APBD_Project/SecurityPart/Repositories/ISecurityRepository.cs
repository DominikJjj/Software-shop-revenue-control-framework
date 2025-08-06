using s27400_APBD_Project.SecurityPart.DTOs;

namespace s27400_APBD_Project.SecurityPart.Repositories;

public interface ISecurityRepository
{
    public Task<bool> IsUserWithThisLoginExist(UserDTO user, CancellationToken token);
    public Task<int> RegisterUser(UserDTO user, CancellationToken token);
    public Task<bool> LoginVerify(LoginDTO login, CancellationToken token);
    public Task<bool> PasswordVerify(LoginDTO login, CancellationToken token);
    public Task<string> GetUserRole(LoginDTO login, CancellationToken token);
    public Task<string> Login(LoginDTO login, string role, CancellationToken token);
    public Task<bool> RefreshCheck(RefreshDTO refresh, CancellationToken token);
    public Task<bool> DateValidate(RefreshDTO refresh, CancellationToken token);
    public Task<string> GetUserRoleByToken(RefreshDTO refresh, CancellationToken token);
    public Task<string> GetUserNameByToken (RefreshDTO refresh, CancellationToken token);
    public Task<string> Refresh(RefreshDTO refresh, string role, string name, CancellationToken token);
}