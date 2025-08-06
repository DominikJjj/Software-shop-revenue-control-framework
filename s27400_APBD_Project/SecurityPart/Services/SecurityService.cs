using s27400_APBD_Project.SecurityPart.DTOs;
using s27400_APBD_Project.SecurityPart.Repositories;

namespace s27400_APBD_Project.SecurityPart.Services;

public class SecurityService : ISecurityService
{
    private readonly ISecurityRepository _securityRepository;

    public SecurityService(ISecurityRepository securityRepository)
    {
        _securityRepository = securityRepository;
    }

    public async Task<int> Register(UserDTO user, CancellationToken token)
    {
        bool verify = await _securityRepository.IsUserWithThisLoginExist(user, token);

        if (verify == true)
        {
            throw new Exception("400 Użytkownik z podanym loginem już istnieje");
        }

        return await _securityRepository.RegisterUser(user, token);
    }

    public async Task<string> Login(LoginDTO login, CancellationToken token)
    {
        bool loginVerify = await _securityRepository.LoginVerify(login, token);
        
        if (loginVerify == false)
        {
            throw new Exception("400 Brak użytkownika o podanej nazwie");
        }

        bool passVerify = await _securityRepository.PasswordVerify(login, token);
        
        if (passVerify == false)
        {
            throw new Exception("400 Niepoprawne hasło");
        }

        string role = await _securityRepository.GetUserRole(login, token);

        return await _securityRepository.Login(login, role, token);
    }

    public async Task<string> RefreshToken(RefreshDTO refresh, CancellationToken token)
    {
        bool userVerify = await _securityRepository.RefreshCheck(refresh, token);

        if (userVerify == false)
        {
            throw new Exception("400 Nie ma użytkownika o podanym refresh tokenie");
        }

        bool dateVerify = await _securityRepository.DateValidate(refresh, token);

        if (dateVerify == false)
        {
            throw new Exception("400 Refresh token jest nieważny");
        }

        string role = await _securityRepository.GetUserRoleByToken(refresh, token);
        string name = await _securityRepository.GetUserNameByToken(refresh, token);

        return await _securityRepository.Refresh(refresh, role, name, token);
    }


}