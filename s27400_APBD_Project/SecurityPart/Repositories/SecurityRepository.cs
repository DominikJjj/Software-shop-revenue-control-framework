using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using s27400_APBD_Project.Entities;
using s27400_APBD_Project.SecurityPart.DTOs;
using s27400_APBD_Project.SecurityPart.Helpers;

namespace s27400_APBD_Project.SecurityPart.Repositories;

public class SecurityRepository : ISecurityRepository
{
    private readonly SoftwareDbContext _context;
    private readonly IConfiguration _configuration;

    public SecurityRepository(SoftwareDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<bool> IsUserWithThisLoginExist(UserDTO user, CancellationToken token)
    {
        var res = await _context.Users
            .FirstOrDefaultAsync(x => x.Login == user.Login, token);

        if (res == null)
        {
            return false;
        }

        return true;
    }

    public async Task<int> RegisterUser(UserDTO user, CancellationToken token)
    {
        var hashedPassAndSalt = SecurityFunctions.GetHashedPasswordAndSalt(user.Password);

        var toAdd = new User()
        {
            Login = user.Login,
            Password = hashedPassAndSalt.Item1,
            Salt = hashedPassAndSalt.Item2,
            DueDateRefreshToken = DateTime.Now.AddHours(12),
            RefreshToken = SecurityFunctions.GenerateRefreshToken(),
            RoleFK = user.RoleId
        };

        await _context.AddAsync(toAdd, token);
        await _context.SaveChangesAsync(token);

        return toAdd.UserId;
    }

    public async Task<bool> LoginVerify(LoginDTO login, CancellationToken token)
    {
        var res = await _context.Users
            .FirstOrDefaultAsync(x => x.Login == login.Login, token);

        if (res == null)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> PasswordVerify(LoginDTO login, CancellationToken token)
    {
        var res = await _context.Users
            .FirstOrDefaultAsync(x => x.Login == login.Login, token);

        if (res.Password != SecurityFunctions.GetHashedPasswordWithSalt(login.Password, res.Salt))
        {
            return false;
        }

        return true;
    }

    public async Task<string> GetUserRole(LoginDTO login, CancellationToken token)
    {
        var res = await _context.Users
            .FirstOrDefaultAsync(x => x.Login == login.Login, token);

        var temp = await _context.Roles
            .FirstOrDefaultAsync(x => x.RoleId == res.RoleFK, token);

        return temp.Name;

    }

    public async Task<string> Login(LoginDTO login, string role, CancellationToken token)
    {
        User tempUser = await _context.Users
            .FirstOrDefaultAsync(x => x.Login == login.Login, token);

        Claim[] userClaims = new[]
        {
            new Claim(ClaimTypes.Name, login.Login),
            new Claim(ClaimTypes.Role, role),
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: _configuration["Issuer"],
            audience: _configuration["Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: credentials
        );

        tempUser.RefreshToken = SecurityFunctions.GenerateRefreshToken();
        tempUser.DueDateRefreshToken = DateTime.Now.AddHours(12);

        await _context.SaveChangesAsync(token);

        return
            $"accessToken: {new JwtSecurityTokenHandler().WriteToken(jwtToken)} refreshToken: {tempUser.RefreshToken}";

    }

    public async Task<bool> RefreshCheck(RefreshDTO refresh, CancellationToken token)
    {
        var res = await _context.Users
            .FirstOrDefaultAsync(x => x.RefreshToken == refresh.Token, token);

        if (res == null)
        {
            return false;
        }

        return true;

    }

    public async Task<bool> DateValidate(RefreshDTO refresh, CancellationToken token)
    {
        var res = await _context.Users
            .FirstOrDefaultAsync(x => x.RefreshToken == refresh.Token, token);

        if (res.DueDateRefreshToken < DateTime.Now)
        {
            return false;
        }

        return true;
    }

    public async Task<string> GetUserRoleByToken(RefreshDTO refresh, CancellationToken token)
    {
        var res = await _context.Users
            .FirstOrDefaultAsync(x => x.RefreshToken == refresh.Token, token);

        var state = await _context.Roles
            .FirstOrDefaultAsync(x => x.RoleId == res.RoleFK, token);

        return state.Name;
    }

    public async Task<string> GetUserNameByToken(RefreshDTO refresh, CancellationToken token)
    {
        var res = await _context.Users
            .FirstOrDefaultAsync(x => x.RefreshToken == refresh.Token, token);

        return res.Login;
    }



    public async Task<string> Refresh(RefreshDTO refresh, string role, string name, CancellationToken token)
    {
        var tempUser = await _context.Users
            .FirstOrDefaultAsync(x => x.RefreshToken == refresh.Token, token);
        
        Claim[] userClaims = new[]
        {
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Role, role)
        };

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: _configuration["Issuer"],
            audience: _configuration["Audience"],
            claims: userClaims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: credentials
        );

        tempUser.RefreshToken = SecurityFunctions.GenerateRefreshToken();
        tempUser.DueDateRefreshToken = DateTime.Now.AddHours(12);

        await _context.SaveChangesAsync(token);
        
        return
            $"accessToken: {new JwtSecurityTokenHandler().WriteToken(jwtToken)} refreshToken: {tempUser.RefreshToken}";
    }
}