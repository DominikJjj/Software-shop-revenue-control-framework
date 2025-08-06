using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using s27400_APBD_Project_Tests.Helpers;
using s27400_APBD_Project_Tests.Setup;
using s27400_APBD_Project.ClientPart.DTOs;
using s27400_APBD_Project.Entities;
using Assert = NUnit.Framework.Assert;

namespace s27400_APBD_Project_Tests.ClientService;

public class ClientServiceTest
{
    private readonly SoftwareDbContext _context;
    private readonly s27400_APBD_Project.ClientPart.Services.ClientService _clientService;

    public ClientServiceTest()
    {
        _context = SoftwareDbContextForTestsFactory.CreateInMemoryDbContext();
        _clientService =
            new s27400_APBD_Project.ClientPart.Services.ClientService(
                new s27400_APBD_Project.ClientPart.Repositories.ClientRepository(_context));
    }


    //AddNewClient()

    [Fact]
    public async Task Add_New_Client_Should_Add_New_Client_To_Database()
    {
        await _clientService.AddNewClient(new ClientAddDTO()
        {
            Email = "example@example.com",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "12345678901",
            PhoneNumber = "123456789"
        }, new CancellationToken());

        var res = await _context.Clients.CountAsync();

        Assert.That(res, Is.EqualTo(1));
    }

    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_Bad_Email_Scheme_Client()
    {
        var res = new ClientAddDTO()
        {
            Email = "exampleexample.com",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "12345678901",
            PhoneNumber = "123456789"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(1));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains("The Email field is not a valid e-mail address.")));
    }

    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_Pesel_And_Phone_Number_Has_Bad_Length_Client()
    {
        var res = new ClientAddDTO()
        {
            Email = "example@example.com",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "123678901",
            PhoneNumber = "123"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(2));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains(
                    "The field PESEL must be a string or array type with a minimum length of '11'.")));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains(
                    "The field PhoneNumber must be a string or array type with a minimum length of '9'.")));
    }

    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_All_Values_Are_Too_Long_Client()
    {
        var res = new ClientAddDTO()
        {
            Email =
                "feiwbqfijb12iob@jfrbewibfiqwbeijfnqweiojnfierbigbri32nfierijreojgb32uibguiobrenunfwiofenjiownfierbngirebuigb3uifneijnqfijqrwebngi3bgiu4benrqiuofneidwnfir3bg3ui2o.com",
            Name =
                "sdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubier",
            Surname =
                "sdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubierweqfewfqew",
            PESEL = "859231789437598712075",
            PhoneNumber = "1234567890"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(5));
        Assert.That(validationResult,
            Has.Exactly(5).Matches<ValidationResult>(x => x.ErrorMessage.Contains("with a maximum length of")));
    }


    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_All_Values_Are_Empty_Client()
    {
        var res = new ClientAddDTO()
        {
            Email = "",
            Name = "",
            Surname = "",
            PESEL = "",
            PhoneNumber = ""
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(5));
        Assert.That(validationResult,
            Has.Exactly(5).Matches<ValidationResult>(x => x.ErrorMessage.Contains("field is required")));
    }

    [Fact]
    public void Validating_Test_Should_Go_Correct_Client()
    {
        var res = new ClientAddDTO()
        {
            Email = "user@example.com",
            Name = "Marek",
            Surname = "Kowalski",
            PESEL = "12345678901",
            PhoneNumber = "123456789"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Empty);
    }

    [Fact]
    public async Task Add_New_Client_Should_Not_Add_New_Client_To_Database_Because_Phone_Number_Contains_Letter()
    {
        var res = Assert.ThrowsAsync<Exception>((async () => await _clientService.AddNewClient(new ClientAddDTO()
        {
            Email = "example@example.com",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "12345678901",
            PhoneNumber = "1234a6789"
        }, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("Number telefonu może zawierać tylko liczby"));
    }

    [Fact]
    public async Task Add_New_Client_Should_Not_Add_New_Client_To_Database_Because_Pesel_Contains_Letter()
    {
        var res = Assert.ThrowsAsync<Exception>((async () => await _clientService.AddNewClient(new ClientAddDTO()
        {
            Email = "example@example.com",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "123456a8901",
            PhoneNumber = "123456789"
        }, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("PESEL może zawierać tylko liczby"));
    }


    [Fact]
    public async Task Add_New_Client_Should_Not_Add_New_Client_To_Database_Because_There_Is_Client_With_The_Same_Pesel()
    {
        await _clientService.AddNewClient(new ClientAddDTO()
        {
            Email = "user@example.com",
            Name = "user",
            Surname = "user",
            PESEL = "12345698901",
            PhoneNumber = "987654321"
        }, new CancellationToken());


        var res = Assert.ThrowsAsync<Exception>((async () => await _clientService.AddNewClient(new ClientAddDTO()
        {
            Email = "example@example.com",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "12345698901",
            PhoneNumber = "123456789"
        }, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("juz istnieje w systemie"));
    }


    //AddNewCompany()

    [Fact]
    public async Task Add_New_Company_Should_Add_New_Company()
    {
        await _clientService.AddNewCompany(new CompanyAddDTO()
        {
            Address = "testAddress",
            Email = "company@compnay.com",
            KRS = "1234567890",
            Name = "testNazwa",
            PhoneNumber = "123456789"
        }, new CancellationToken());

        var res = await _context.Companies.CountAsync();

        Assert.That(res, Is.EqualTo(1));
    }

    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_Bad_Email_Scheme_Compnay()
    {
        var res = new CompanyAddDTO()
        {
            Email = "exampleexample.com",
            Name = "testName",
            Address = "testAddress",
            KRS = "1234567890",
            PhoneNumber = "123456789"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(1));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains("The Email field is not a valid e-mail address.")));
    }


    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_KRS_And_Phone_Number_Has_Bad_Length_Company()
    {
        var res = new CompanyAddDTO()
        {
            Email = "example@example.com",
            Name = "testName",
            Address = "testAddress",
            KRS = "12234",
            PhoneNumber = "123"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(2));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains(
                    "The field KRS must be a string or array type with a minimum length of '10'.")));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains(
                    "The field PhoneNumber must be a string or array type with a minimum length of '9'.")));
    }

    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_All_Values_Are_Too_Long_Company()
    {
        var res = new CompanyAddDTO()
        {
            Email =
                "feiwbqfijb12iob@jfrbewibfiqwbeijfnqweiojnfierbigbri32nfierijreojgb32uibguiobrenunfwiofenjiownfierbngirebuigb3uifneijnqfijqrwebngi3bgiu4benrqiuofneidwnfir3bg3ui2o.com",
            Name =
                "sdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubier",
            Address =
                "sdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubierweqfewfqewsdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubierweqfewfqewsdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubierweqfewfqew",
            KRS = "859231789437598712075",
            PhoneNumber = "1234567890"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(5));
        Assert.That(validationResult,
            Has.Exactly(5).Matches<ValidationResult>(x => x.ErrorMessage.Contains("with a maximum length of")));
    }


    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_All_Values_Are_Empty_Company()
    {
        var res = new CompanyAddDTO()
        {
            Email = "",
            Name = "",
            Address = "",
            KRS = "",
            PhoneNumber = ""
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(5));
        Assert.That(validationResult,
            Has.Exactly(5).Matches<ValidationResult>(x => x.ErrorMessage.Contains("field is required")));
    }


    [Fact]
    public void Validating_Test_Should_Go_Correct_Company()
    {
        var res = new CompanyAddDTO()
        {
            Email = "user@example.com",
            Name = "CompanyTest",
            Address = "ul. Miodowa 12 Warszawa",
            KRS = "1234567890",
            PhoneNumber = "123456789"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Empty);
    }


    [Fact]
    public async Task Add_New_Company_Should_Not_Add_New_Company_To_Database_Because_Phone_Number_Contains_Letter()
    {
        var res = Assert.ThrowsAsync<Exception>((async () => await _clientService.AddNewCompany(new CompanyAddDTO()
        {
            Email = "example@example.com",
            Name = "companyName",
            Address = "ul. Miodowa 12 Warszawa",
            KRS = "1234567890",
            PhoneNumber = "1234a6789"
        }, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("Number telefonu może zawierać tylko liczby"));
    }


    [Fact]
    public async Task Add_New_Company_Should_Not_Add_New_Company_To_Database_Because_KRS_Contains_Letter()
    {
        var res = Assert.ThrowsAsync<Exception>((async () => await _clientService.AddNewCompany(new CompanyAddDTO()
        {
            Email = "example@example.com",
            Name = "CompanyName",
            Address = "ul. Miodowa 12 Warszawa",
            KRS = "123456789p",
            PhoneNumber = "123456789"
        }, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("KRS może zawierać tylko liczby"));
    }


    [Fact]
    public async Task
        Add_New_Company_Should_Not_Add_New_Company_To_Database_Because_There_Is_Company_With_The_Same_KRS()
    {
        await _clientService.AddNewCompany(new CompanyAddDTO()
        {
            Email = "aaaa@example.com",
            Name = "aaaa",
            Address = "ul. Miodowa 12 Warszawa",
            KRS = "1234567890",
            PhoneNumber = "987654321"
        }, new CancellationToken());


        var res = Assert.ThrowsAsync<Exception>((async () => await _clientService.AddNewCompany(new CompanyAddDTO()
        {
            Email = "example@example.com",
            Name = "example",
            Address = "ul. Miodowa 32 Warszawa",
            KRS = "1234567890",
            PhoneNumber = "123456789"
        }, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("juz istnieje w systemie"));
    }


    //SoftDeleteClient()

    [Fact]
    public async Task Soft_Delete_Client_Should_Delete_Client()
    {
        var client = new Client()
        {
            Email = "user@wp.pl",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "1234567890",
            PhoneNumber = "123456789",
            IsDeleted = false
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        await _clientService.SoftDeleteClient(client.ClientId, new CancellationToken());

        var verify = await _context.Clients
            .FirstOrDefaultAsync(x => x.ClientId == client.ClientId);

        Assert.That(verify.IsDeleted, Is.EqualTo(true));
    }

    [Fact]
    public async Task Soft_Delete_Client_Should_Not_Delete_Client_Because_Client_Not_Exists()
    {
        var client = new Client()
        {
            Email = "user@wp.pl",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "1234567890",
            PhoneNumber = "123456789",
            IsDeleted = false
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();


        var res = Assert.ThrowsAsync<Exception>((async () =>
            await _clientService.SoftDeleteClient(client.ClientId + 1, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("nie jest wprowadzony w systemie"));
    }

    [Fact]
    public async Task Soft_Delete_Client_Should_Not_Delete_Client_Because_Client_Is_Already_Deleted()
    {
        var client = new Client()
        {
            Email = "user@wp.pl",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "1234567890",
            PhoneNumber = "123456789",
            IsDeleted = true
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();


        var res = Assert.ThrowsAsync<Exception>((async () =>
            await _clientService.SoftDeleteClient(client.ClientId, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("został już usnięty wcześniej"));
    }


    //UpdateClient()
    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_Bad_Email_Scheme_Client_Put()
    {
        var res = new ClientPutDTO()
        {
            Email = "exampleexample.com",
            Name = "Adam",
            Surname = "Nowak",
            PhoneNumber = "123456789"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(1));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains("The Email field is not a valid e-mail address.")));
    }

    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_Phone_Number_Has_Bad_Length_Client_Put()
    {
        var res = new ClientPutDTO()
        {
            Email = "example@example.com",
            Name = "Adam",
            Surname = "Nowak",
            PhoneNumber = "123"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(1));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains(
                    "The field PhoneNumber must be a string or array type with a minimum length of '9'.")));
    }


    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_All_Values_Are_Too_Long_Client_Put()
    {
        var res = new ClientPutDTO()
        {
            Email =
                "feiwbqfijb12iob@jfrbewibfiqwbeijfnqweiojnfierbigbri32nfierijreojgb32uibguiobrenunfwiofenjiownfierbngirebuigb3uifneijnqfijqrwebngi3bgiu4benrqiuofneidwnfir3bg3ui2o.com",
            Name =
                "sdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubier",
            Surname =
                "sdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubierweqfewfqew",
            PhoneNumber = "1234567890"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(4));
        Assert.That(validationResult,
            Has.Exactly(4).Matches<ValidationResult>(x => x.ErrorMessage.Contains("with a maximum length of")));
    }

    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_All_Values_Are_Empty_Client_Put()
    {
        var res = new ClientPutDTO()
        {
            Email = "",
            Name = "",
            Surname = "",
            PhoneNumber = ""
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(4));
        Assert.That(validationResult,
            Has.Exactly(4).Matches<ValidationResult>(x => x.ErrorMessage.Contains("field is required")));
    }

    [Fact]
    public void Validating_Test_Should_Go_Correct_Client_Put()
    {
        var res = new ClientPutDTO()
        {
            Email = "user@example.com",
            Name = "Marek",
            Surname = "Kowalski",
            PhoneNumber = "123456789"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Empty);
    }

    [Fact]
    public async Task Update_Client_Should_Go_Wrong_Because_Phone_Number_Contains_Letter()
    {
        var client = new Client()
        {
            Email = "user@wp.pl",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "1234567890",
            PhoneNumber = "123456789",
            IsDeleted = false
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        var newClientPutDto = new ClientPutDTO()
        {
            Email = "user@wp.pl",
            Name = "Adam",
            Surname = "Nowak",
            PhoneNumber = "12ab56789",
        };

        var res = Assert.ThrowsAsync<Exception>((async () =>
            await _clientService.UpdateClient(client.ClientId, newClientPutDto, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("Number telefonu może zawierać tylko liczby"));
    }

    [Fact]
    public async Task Update_Client_Should_Go_Wrong_Because_Client_Not_Exist()
    {
        var client = new Client()
        {
            Email = "user@wp.pl",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "1234567890",
            PhoneNumber = "123456789",
            IsDeleted = false
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        var newClientPutDto = new ClientPutDTO()
        {
            Email = "user@wp.pl",
            Name = "Adam",
            Surname = "Nowak",
            PhoneNumber = "123456789",
        };

        var res = Assert.ThrowsAsync<Exception>((async () =>
            await _clientService.UpdateClient(client.ClientId + 1, newClientPutDto, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("nie jest wprowadzony w systemie"));
    }


    [Fact]
    public async Task Update_Client_Should_Go_Well()
    {
        var client = new Client()
        {
            Email = "user@wp.pl",
            Name = "Adam",
            Surname = "Nowak",
            PESEL = "1234567890",
            PhoneNumber = "123456789",
            IsDeleted = false
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        var newClientPutDto = new ClientPutDTO()
        {
            Email = "new@wp.pl",
            Name = "new",
            Surname = "new",
            PhoneNumber = "999888777",
        };

        await _clientService.UpdateClient(client.ClientId, newClientPutDto, new CancellationToken());

        Assert.That(client.Email, Is.EqualTo("new@wp.pl"));
        Assert.That(client.Name, Is.EqualTo("new"));
        Assert.That(client.Surname, Is.EqualTo("new"));
        Assert.That(client.PhoneNumber, Is.EqualTo("999888777"));
    }
    
    //UpdateCompany()
    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_Bad_Email_Scheme_Compnay_Put()
    {
        var res = new CompanyPutDTO()
        {
            Email = "exampleexample.com",
            Name = "testName",
            Address = "testAddress",
            PhoneNumber = "123456789"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(1));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains("The Email field is not a valid e-mail address.")));
    }
    
    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_Phone_Number_Has_Bad_Length_Company_Put()
    {
        var res = new CompanyPutDTO()
        {
            Email = "example@example.com",
            Name = "testName",
            Address = "testAddress",
            PhoneNumber = "123"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(1));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains(
                    "The field PhoneNumber must be a string or array type with a minimum length of '9'.")));
    }
    
    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_All_Values_Are_Too_Long_Company_Put()
    {
        var res = new CompanyPutDTO()
        {
            Email =
                "feiwbqfijb12iob@jfrbewibfiqwbeijfnqweiojnfierbigbri32nfierijreojgb32uibguiobrenunfwiofenjiownfierbngirebuigb3uifneijnqfijqrwebngi3bgiu4benrqiuofneidwnfir3bg3ui2o.com",
            Name =
                "sdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubier",
            Address =
                "sdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubierweqfewfqewsdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubierweqfewfqewsdofgeirjgnijenrwjgnfeijrnjfieirwfjiehwrifbedinfviewnfionewofnqweiofnierwbgfberuowfhiorweqfhouwebfgiherbgiurheouwifqhiouwerqhnfiuqweubierweqfewfqew",
            PhoneNumber = "1234567890"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(4));
        Assert.That(validationResult,
            Has.Exactly(4).Matches<ValidationResult>(x => x.ErrorMessage.Contains("with a maximum length of")));
    }
    
    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_All_Values_Are_Empty_Company_Put()
    {
        var res = new CompanyPutDTO()
        {
            Email = "",
            Name = "",
            Address = "",
            PhoneNumber = ""
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(4));
        Assert.That(validationResult,
            Has.Exactly(4).Matches<ValidationResult>(x => x.ErrorMessage.Contains("field is required")));
    }


    [Fact]
    public void Validating_Test_Should_Go_Correct_Company_Put()
    {
        var res = new CompanyPutDTO()
        {
            Email = "user@example.com",
            Name = "CompanyTest",
            Address = "ul. Miodowa 12 Warszawa",
            PhoneNumber = "123456789"
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Empty);
    }
    
    [Fact]
    public async Task Update_Company_Should_Go_Wrong_Because_Phone_Number_Contains_Letter()
    {
        var company = new Company()
        {
            Email = "user@wp.pl",
            Name = "companyName",
            Address = "ul. Miodowa 12 Warszawa",
            KRS = "1234567890",
            PhoneNumber = "123456789",
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        var newCompany = new CompanyPutDTO()
        {
            Email = "user@wp.pl",
            Name = "Adam",
            Address = "ul. Miodowa 12 Warszawa",
            PhoneNumber = "12ab56789",
        };

        var res = Assert.ThrowsAsync<Exception>((async () =>
            await _clientService.UpdateCompany(company.CompanyId, newCompany, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("Number telefonu może zawierać tylko liczby"));
    }
    
    [Fact]
    public async Task Update_Company_Should_Go_Wrong_Because_Company_Not_Exist()
    {
        var company = new Company()
        {
            Email = "user@wp.pl",
            Name = "companyName",
            Address = "ul. Miodowa 12 Warszawa",
            KRS = "1234567890",
            PhoneNumber = "123456789",
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        var newCompany = new CompanyPutDTO()
        {
            Email = "user@wp.pl",
            Name = "Adam",
            Address = "ul. Miodowa 12 Warszawa",
            PhoneNumber = "123356789",
        };

        var res = Assert.ThrowsAsync<Exception>((async () =>
            await _clientService.UpdateCompany(company.CompanyId+1, newCompany, new CancellationToken())));

        Assert.That(res.Message, Does.Contain("nie jest wprowadzony w systemie"));
    }


    [Fact]
    public async Task Update_Company_Should_Go_Well()
    {
        var company = new Company()
        {
            Email = "user@wp.pl",
            Name = "companyName",
            Address = "ul. Miodowa 12 Warszawa",
            KRS = "1234567890",
            PhoneNumber = "123456789",
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        var newCompany = new CompanyPutDTO()
        {
            Email = "new@wp.pl",
            Name = "new",
            Address = "ul. Miodowa 32 Warszawa",
            PhoneNumber = "111222333",
        };
        await _clientService.UpdateCompany(company.CompanyId, newCompany, new CancellationToken());

        Assert.That(company.Email, Is.EqualTo("new@wp.pl"));
        Assert.That(company.Name, Is.EqualTo("new"));
        Assert.That(company.Address, Is.EqualTo("ul. Miodowa 32 Warszawa"));
        Assert.That(company.PhoneNumber, Is.EqualTo("111222333"));

    }
}