using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using s27400_APBD_Project_Tests.Helpers;
using s27400_APBD_Project_Tests.Setup;
using s27400_APBD_Project.Entities;
using s27400_APBD_Project.SecurityPart.DTOs;
using Assert = NUnit.Framework.Assert;


namespace s27400_APBD_Project_Tests.SecurityService;

public class SecurityServiceTest
{
    private readonly SoftwareDbContext _context;
    private readonly s27400_APBD_Project.SecurityPart.Services.SecurityService _securityService;
    private readonly IConfiguration _configuration;

    public SecurityServiceTest()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        _context = SoftwareDbContextForTestsFactory.CreateInMemoryDbContext();
        _configuration = builder.Build();
        _securityService = new s27400_APBD_Project.SecurityPart.Services.SecurityService(
            new s27400_APBD_Project.SecurityPart.Repositories.SecurityRepository(_context, _configuration));
    }
    
    //Register()
    [Fact]
    public void Validating_User_DTO_Should_Not_Pass_Because_All_Values_Are_Empty()
    {
        var model = new UserDTO()
        {
            Login = "",
            Password = "",
            RoleId = 1
        };

        var validation = ModelValidationHelper.Validate(model);
        
        Assert.That(validation, Is.Not.Empty);
        Assert.That(validation.Count, Is.EqualTo(2));
        Assert.That(validation, Has.Exactly(2).Matches<ValidationResult>(x => x.ErrorMessage.Contains("field is required")));

    }
    
    [Fact]
    public void Validating_User_DTO_Should_Not_Pass_Because_RoleId_Out_Of_Range()
    {
        var model = new UserDTO()
        {
            Login = "sdaf",
            Password = "grew",
            RoleId = 3
        };

        var validation = ModelValidationHelper.Validate(model);
        
        Assert.That(validation, Is.Not.Empty);
        Assert.That(validation.Count, Is.EqualTo(1));
        Assert.That(validation, Has.Exactly(1).Matches<ValidationResult>(x => x.ErrorMessage.Contains("must be between")));

    }
    
    [Fact]
    public void Validating_User_DTO_Should_Pass()
    {
        var model = new UserDTO()
        {
            Login = "sdaf",
            Password = "grew",
            RoleId = 2
        };

        var validation = ModelValidationHelper.Validate(model);
        
        Assert.That(validation, Is.Empty);

    }
    
    [Fact]
    public async Task Register_Should_Not_Work_Because_There_Is_Already_User_With_This_Login()
    {
        var user = new User()
        {
            Login = "Test123",
            Password = "test123",
            Salt = "123test",
            RefreshToken = "qwert345",
            DueDateRefreshToken = DateTime.Now.AddHours(12),
            RoleFK = 1
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var newUser = new UserDTO()
        {
            Login = "Test123",
            Password = "test123",
            RoleId = 1
        };

        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _securityService.Register(newUser, new CancellationToken()));
        
        Assert.That(res.Message, Does.Contain("Użytkownik z podanym loginem już istnieje"));
    }
    
    [Fact]
    public async Task Register_Should_Work_()
    {
        var user = new User()
        {
            Login = "Test123",
            Password = "test123",
            Salt = "123test",
            RefreshToken = "qwert345",
            DueDateRefreshToken = DateTime.Now.AddHours(12),
            RoleFK = 1
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var newUser = new UserDTO()
        {
            Login = "nowyUser",
            Password = "nowehaslo",
            RoleId = 1
        };

        await _securityService.Register(newUser, new CancellationToken());

        var res = await _context.Users.CountAsync();
        
        Assert.That(res, Is.EqualTo(2));
    }
    
    //Login()
    [Fact]
    public void Validate_Model_LoginDTO_Should_Not_Pass_Empty_Values()
    {
        var model = new LoginDTO()
        {
            Login = "",
            Password = ""
        };
        
        var validation = ModelValidationHelper.Validate(model);
        
        Assert.That(validation, Is.Not.Empty);
        Assert.That(validation.Count, Is.EqualTo(2));
        Assert.That(validation, Has.Exactly(2).Matches<ValidationResult>(x => x.ErrorMessage.Contains("field is required")));
    }
    
    [Fact]
    public void Validate_Model_LoginDTO_Should_Not_Pass_Login_Too_Long()
    {
        var model = new LoginDTO()
        {
            Login = "dnfijenuifewiufioerwuhfuiqbdfijeoqwfhowiefhiowqubgirbeuwghfewiufnoqwndjifnbqswifhberifgbewqiyfhdqwisufhwejibghie",
            Password = "fe13rf213e"
        };
        
        var validation = ModelValidationHelper.Validate(model);
        
        Assert.That(validation, Is.Not.Empty);
        Assert.That(validation.Count, Is.EqualTo(1));
        Assert.That(validation, Has.Exactly(1).Matches<ValidationResult>(x => x.ErrorMessage.Contains("maximum length")));
    }
    
    [Fact]
    public void Validate_Model_LoginDTO_Should_Pass()
    {
        var model = new LoginDTO()
        {
            Login = "test123",
            Password = "123test"
        };
        
        var validation = ModelValidationHelper.Validate(model);
        
        Assert.That(validation, Is.Empty);
    }

    [Fact]
    public async Task Login_Not_Work_No_Login()
    {
        var user = new User()
        {
            Login = "Test123",
            Password = "test123",
            Salt = "123test",
            RefreshToken = "qwert345",
            DueDateRefreshToken = DateTime.Now.AddHours(12),
            RoleFK = 1
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var login = new LoginDTO()
        {
            Login = "asd123",
            Password = "test123"
        };
        
        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _securityService.Login(login, new CancellationToken()));
        
        Assert.That(res.Message, Does.Contain("Brak użytkownika o podanej nazwie"));
        
    }
    
    [Fact]
    public async Task Login_Not_Work_Bad_Password()
    {
        var user = new User()
        {
            Login = "Test123",
            Password = "test123",
            Salt = "",
            RefreshToken = "qwert345",
            DueDateRefreshToken = DateTime.Now.AddHours(12),
            RoleFK = 1
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var login = new LoginDTO()
        {
            Login = "Test123",
            Password = "asd123"
        };
        
        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _securityService.Login(login, new CancellationToken()));
        
        Assert.That(res.Message, Does.Contain("Niepoprawne hasło"));
        
    }
    
    [Fact]
    public async Task Login_Work()
    {
        var role = new Role()
        {
            Name = "role2"
        };

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        
        var user = new UserDTO()
        {
            Login = "admin",
            Password = "admin",
            RoleId = role.RoleId
        };

        await _securityService.Register(user, new CancellationToken());

        var login = new LoginDTO()
        {
            Login = "admin",
            Password = "admin"
        };
        
       var res = await _securityService.Login(login, new CancellationToken());
        
        Assert.That(res, Does.Contain("accessToken"));
    }
    
    //Refresh()
    [Fact]
    public void Validate_Model_Not_Pass_Empty_Values()
    {
        var model = new RefreshDTO()
        {
            Token = ""
        };
        
        var validation = ModelValidationHelper.Validate(model);
        
        Assert.That(validation, Is.Not.Empty);
        Assert.That(validation.Count, Is.EqualTo(1));
        Assert.That(validation, Has.Exactly(1).Matches<ValidationResult>(x => x.ErrorMessage.Contains("field is required")));
    }
    
    [Fact]
    public void Validate_Model_Pass()
    {
        var model = new RefreshDTO()
        {
            Token = "fnui23nre2ibfnu3oer280einowfrq"
        };
        
        var validation = ModelValidationHelper.Validate(model);
        
        Assert.That(validation, Is.Empty);
    }

    [Fact]
    public async Task Refresh_Not_Work_Because_Wrong_Token()
    {
        var role = new Role()
        {
            Name = "role2"
        };

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        
        var user = new UserDTO()
        {
            Login = "admin",
            Password = "admin",
            RoleId = role.RoleId
        };

        await _securityService.Register(user, new CancellationToken());

        var getUser = await _context.Users.FirstOrDefaultAsync(x => x.Login == user.Login);

        RefreshDTO refresh = new RefreshDTO()
        {
            Token = getUser.RefreshToken + "abd"
        };
        
        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _securityService.RefreshToken(refresh, new CancellationToken()));
        
        Assert.That(res.Message, Does.Contain("Nie ma użytkownika o podanym refresh tokenie"));
    }
    
    [Fact]
    public async Task Refresh_Not_Work_Because_Expired_Token()
    {
        var role = new Role()
        {
            Name = "role2"
        };

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        
        var user = new UserDTO()
        {
            Login = "admin",
            Password = "admin",
            RoleId = role.RoleId
        };

        await _securityService.Register(user, new CancellationToken());

        var getUser = await _context.Users.FirstOrDefaultAsync(x => x.Login == user.Login);

        getUser.DueDateRefreshToken = new DateTime(2024, 1, 1);
        await _context.SaveChangesAsync();

        RefreshDTO refresh = new RefreshDTO()
        {
            Token = getUser.RefreshToken
        };
        
        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _securityService.RefreshToken(refresh, new CancellationToken()));
        
        Assert.That(res.Message, Does.Contain("Refresh token jest nieważny"));
    }
    
    [Fact]
    public async Task Refresh_Work()
    {
        var role = new Role()
        {
            Name = "role2"
        };

        await _context.Roles.AddAsync(role);
        await _context.SaveChangesAsync();
        
        var user = new UserDTO()
        {
            Login = "admin",
            Password = "admin",
            RoleId = role.RoleId
        };

        await _securityService.Register(user, new CancellationToken());

        var getUser = await _context.Users.FirstOrDefaultAsync(x => x.Login == user.Login);

        RefreshDTO refresh = new RefreshDTO()
        {
            Token = getUser.RefreshToken
        };

        var res = await _securityService.RefreshToken(refresh, new CancellationToken());
        
        Assert.That(res, Does.Contain("accessToken"));
    }
    
    
}