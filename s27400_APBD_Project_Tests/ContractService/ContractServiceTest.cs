using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using s27400_APBD_Project_Tests.Helpers;
using s27400_APBD_Project_Tests.Setup;
using s27400_APBD_Project.ContractPart.DTOs;
using s27400_APBD_Project.Entities;
using Assert = NUnit.Framework.Assert;

namespace s27400_APBD_Project_Tests.ContractService;

public class ContractServiceTest
{
    private readonly SoftwareDbContext _context;
    private readonly s27400_APBD_Project.ContractPart.Services.ContractService _contractService;

    public ContractServiceTest()
    {
        _context = SoftwareDbContextForTestsFactory.CreateInMemoryDbContext();
        _contractService = new s27400_APBD_Project.ContractPart.Services.ContractService(
            new s27400_APBD_Project.ContractPart.Repositories.ContractRepository(_context));
    }

    //CreateContract()
    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_Firma_czy_Klient_Value_Out_Of_Range()
    {
        var res = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = 1,
                Firma_czy_Klient = "test"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = 1,
                    UpdateTime = 2
                }
            }
        };

        var validationResult = ModelValidationHelper.Validate(res.ClientInfo);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(1));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x =>
                x.ErrorMessage.Contains("Dopuszczalne wartości w tym polu to Klient lub Firma")));
    }

    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_ClientInfo_Softwares_Null()
    {
        var res = new ContractDTO()
        {
            ClientInfo = null,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = null
        };

        var validationResult = ModelValidationHelper.Validate(res);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(2));
        Assert.That(validationResult,
            Has.Exactly(2).Matches<ValidationResult>(x => x.ErrorMessage.Contains("field is required")));
    }

    [Fact]
    public void Validating_Test_Should_Go_Wrong_Because_Softwares_Bad_Value_Update_Time()
    {
        var res = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = 1,
                Firma_czy_Klient = "test"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = 1,
                    UpdateTime = 6
                }
            }
        };

        var validationResult = ModelValidationHelper.Validate(res.Softwares[0]);

        Assert.That(validationResult, Is.Not.Empty);
        Assert.That(validationResult.Count, Is.EqualTo(1));
        Assert.That(validationResult,
            Has.Exactly(1).Matches<ValidationResult>(x => x.ErrorMessage.Contains("must be between")));
    }

    [Fact]
    public void Validating_Test_Should_Go_Well()
    {
        var res = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = 1,
                Firma_czy_Klient = "Klient"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = 1,
                    UpdateTime = 3
                },
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = 2,
                    UpdateTime = 3
                }
            }
        };

        var validationResult = ModelValidationHelper.Validate(res.Softwares[0]);

        Assert.That(validationResult, Is.Empty);
    }

    [Fact]
    public async Task Create_Contract_Cant_Be_Created_Because_Time_Is_Too_Short()
    {
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = 1,
                Firma_czy_Klient = "Klient"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(2),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = 1,
                    UpdateTime = 2
                }
            }
        };

        var res = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _contractService.CreateContract(toAdd, new CancellationToken());
        });
        
        Assert.That(res.Message, Does.Contain("Przedział czasowy powinien wynosić między 3 a 30 dni"));
    }
    
    [Fact]
    public async Task Create_Contract_Cant_Be_Created_Because_Time_Is_Too_Long()
    {
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = 1,
                Firma_czy_Klient = "Klient"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(33),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = 1,
                    UpdateTime = 2
                }
            }
        };

        var res = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _contractService.CreateContract(toAdd, new CancellationToken());
        });
        
        Assert.That(res.Message, Does.Contain("Przedział czasowy powinien wynosić między 3 a 30 dni"));
    }
    
    [Fact]
    public async Task Create_Contract_Cant_Be_Created_Because_There_Is_No_Software_Added()
    {
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = 1,
                Firma_czy_Klient = "Klient"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
        };

        var res = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _contractService.CreateContract(toAdd, new CancellationToken());
        });
        
        Assert.That(res.Message, Does.Contain("W skład kontraktu muszą wchodzić programy"));
    }
    
    [Fact]
    public async Task Create_Contract_Cant_Be_Created_Because_There_Is_No_Company_In_Database()
    {
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = 1,
                Firma_czy_Klient = "Company"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = 1,
                    UpdateTime = 2
                }
            }
        };

        var res = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _contractService.CreateContract(toAdd, new CancellationToken());
        });
        
        Assert.That(res.Message, Does.Contain("nie istnieje w systemie"));
    }
    
    [Fact]
    public async Task Create_Contract_Cant_Be_Created_Because_There_Is_No_Client_In_Database()
    {
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = 1,
                Firma_czy_Klient = "Klient"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = 1,
                    UpdateTime = 2
                }
            }
        };

        var res = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _contractService.CreateContract(toAdd, new CancellationToken());
        });
        
        Assert.That(res.Message, Does.Contain("nie istnieje w systemie"));
    }
    
    [Fact]
    public async Task Create_Contract_Cant_Be_Created_Because_There_Is_No_Software_In_Database()
    {

        var client = new Client()
        {
            Email = "user@wp.pl",
            IsDeleted = false,
            Name = "Marek",
            PESEL = "12345678900",
            PhoneNumber = "123456789",
            Surname = "Nowak"
        };

        await _context.Clients.AddAsync(client, new CancellationToken());
        await _context.SaveChangesAsync(new CancellationToken());
        
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = 1,
                Firma_czy_Klient = "Klient"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = 1,
                    UpdateTime = 2
                }
            }
        };

        var res = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _contractService.CreateContract(toAdd, new CancellationToken());
        });
        
        Assert.That(res.Message, Does.Contain("Podany program nie wystepuje w bazie"));
    }
    
    
    [Fact]
    public async Task Create_Contract_Cant_Be_Created_Because_User_Already_Have_License_For_Software()
    {

        var company = new Company()
        {
            Email = "user@wp.pl",
            Address = "address",
            KRS = "1234567890",
            PhoneNumber = "123456789",
            Name = "test"
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 1000.01M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();

        var state = new State()
        {
            StateId = 1,
            Name = "stataename"
        };
        
        await _context.States.AddAsync(state);
        await _context.SaveChangesAsync();

        var contract = new Contract()
        {
            EndDate = new DateTime(2024, 1, 1),
            StartDate = new DateTime(2024, 1, 7),
            Price = 2000.01M,
            StateFK = 2,
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var conSoft = new ContractSoftware()
        {
            ContractFK = contract.ContractId,
            SoftwareSystemFK = software.SoftwareId,
            PriceInContract = 2000.01M,
            UpdateTime = 2,
            Version = "versioncon"
        };

        await _context.ContractSoftwares.AddAsync(conSoft);
        await _context.SaveChangesAsync();

        var pay = new Payment()
        {
            CompanyFK = company.CompanyId,
            ContractFK = contract.ContractId,
            ValuePaid = 2000.01M
        };

        await _context.Payments.AddAsync(pay);
        await _context.SaveChangesAsync();
        
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = company.CompanyId,
                Firma_czy_Klient = "Firma"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = software.SoftwareId,
                    UpdateTime = 2
                }
            }
        };

        var res = Assert.ThrowsAsync<Exception>(async () =>
        {
            await _contractService.CreateContract(toAdd, new CancellationToken());
        });
        
        Assert.That(res.Message, Does.Contain("Zamawiający posiada juz aktywne licencje na wybrane z zamawianych programów"));
    }
    
    
        [Fact]
    public async Task Create_Contract_Can_Be_Created_New_Company()
    {

        var company = new Company()
        {
            Email = "user@wp.pl",
            Address = "address",
            KRS = "1234567890",
            PhoneNumber = "123456789",
            Name = "test"
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 1000.01M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();

        
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = company.CompanyId,
                Firma_czy_Klient = "Firma"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = software.SoftwareId,
                    UpdateTime = 2
                }
            }
        };
        
            await _contractService.CreateContract(toAdd, new CancellationToken());


        var numOfRes = await _context.Contracts.CountAsync();

        var resNum = await _context.Contracts
            .FirstOrDefaultAsync(x => x.ContractId == x.ContractId);

        var resNumOfContractSoft = await _context.ContractSoftwares
            .FirstOrDefaultAsync(x => x.ContractFK == resNum.ContractId);

        var resOfPayment = await _context.Payments
            .FirstOrDefaultAsync(x => x.ContractFK == resNum.ContractId);

        
        
        Assert.That(numOfRes, Is.EqualTo(1));
        Assert.That(resNum.Price, Is.EqualTo(2000.01M));
        Assert.That(resNumOfContractSoft.PriceInContract, Is.EqualTo(2000.01M));
        Assert.That(resOfPayment.ValuePaid, Is.EqualTo(0));
    }
    
    
    [Fact]
    public async Task Create_Contract_Can_Be_Created_Regular_Company()
    {

        var company = new Company()
        {
            Email = "user@wp.pl",
            Address = "address",
            KRS = "1234567890",
            PhoneNumber = "123456789",
            Name = "test"
        };

        await _context.Companies.AddAsync(company);
        await _context.SaveChangesAsync();

        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 1000.01M,
            Version = "version"
        };

        var software2 = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "decs1",
            Name = "Name12",
            Price = 2000.01M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();
        await _context.SoftwareSystems.AddAsync(software2);
        await _context.SaveChangesAsync();

        var state = new State()
        {
            StateId = 1,
            Name = "stataename"
        };
        
        await _context.States.AddAsync(state);
        await _context.SaveChangesAsync();

        var contract = new Contract()
        {
            EndDate = new DateTime(2024, 1, 1),
            StartDate = new DateTime(2024, 1, 7),
            Price = 2000.01M,
            StateFK = 2,
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var conSoft = new ContractSoftware()
        {
            ContractFK = contract.ContractId,
            SoftwareSystemFK = software.SoftwareId,
            PriceInContract = 2000.01M,
            UpdateTime = 2,
            Version = "versioncon"
        };

        await _context.ContractSoftwares.AddAsync(conSoft);
        await _context.SaveChangesAsync();

        var pay = new Payment()
        {
            CompanyFK = company.CompanyId,
            ContractFK = contract.ContractId,
            ValuePaid = 2000.01M
        };

        await _context.Payments.AddAsync(pay);
        await _context.SaveChangesAsync();

        
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = company.CompanyId,
                Firma_czy_Klient = "Firma"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = software2.SoftwareId,
                    UpdateTime = 1
                }
            }
        };
        
            await _contractService.CreateContract(toAdd, new CancellationToken());


        var numOfRes = await _context.Contracts.CountAsync();

        var resNum = await _context.Contracts
            .FirstOrDefaultAsync(x => x.StartDate == toAdd.StartDate);

        var resNumOfContractSoft = await _context.ContractSoftwares
            .FirstOrDefaultAsync(x => x.ContractFK == resNum.ContractId);

        var resOfPayment = await _context.Payments
            .FirstOrDefaultAsync(x => x.ContractFK == resNum.ContractId);
        
        
        
        Assert.That(numOfRes, Is.EqualTo(2));
        Assert.That(resNum.Price, Is.EqualTo(1900.0095M));
        Assert.That(resNumOfContractSoft.PriceInContract, Is.EqualTo(1900.0095M));
        Assert.That(resOfPayment.ValuePaid, Is.EqualTo(0));
    }
    
    
           [Fact]
    public async Task Create_Contract_Can_Be_Created_New_Client()
    {

        var client = new Client()
        {
            Email = "user@wp.pl",
            Surname = "testSur",
            PESEL = "12345678900",
            PhoneNumber = "123456789",
            Name = "test"
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 1000.01M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();

        
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = client.ClientId,
                Firma_czy_Klient = "Klient"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = software.SoftwareId,
                    UpdateTime = 2
                }
            }
        };
        
            await _contractService.CreateContract(toAdd, new CancellationToken());


        var numOfRes = await _context.Contracts.CountAsync();

        var resNum = await _context.Contracts
            .FirstOrDefaultAsync(x => x.ContractId == x.ContractId);

        var resNumOfContractSoft = await _context.ContractSoftwares
            .FirstOrDefaultAsync(x => x.ContractFK == resNum.ContractId);

        var resOfPayment = await _context.Payments
            .FirstOrDefaultAsync(x => x.ContractFK == resNum.ContractId);

        
        
        Assert.That(numOfRes, Is.EqualTo(1));
        Assert.That(resNum.Price, Is.EqualTo(2000.01M));
        Assert.That(resNumOfContractSoft.PriceInContract, Is.EqualTo(2000.01M));
        Assert.That(resOfPayment.ValuePaid, Is.EqualTo(0));
    }
    
    
        [Fact]
    public async Task Create_Contract_Can_Be_Created_Regular_Client()
    {

        var client = new Client()
        {
            Email = "user@wp.pl",
            Surname = "testSurname",
            PESEL = "12312312312",
            PhoneNumber = "123456789",
            Name = "test"
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 1000.01M,
            Version = "version"
        };

        var software2 = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "decs1",
            Name = "Name12",
            Price = 4000.01M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();
        await _context.SoftwareSystems.AddAsync(software2);
        await _context.SaveChangesAsync();

        var state = new State()
        {
            StateId = 2,
            Name = "stataename"
        };
        
        await _context.States.AddAsync(state);
        await _context.SaveChangesAsync();

        var contract = new Contract()
        {
            EndDate = new DateTime(2024, 1, 1),
            StartDate = new DateTime(2024, 1, 7),
            Price = 2000.01M,
            StateFK = 2,
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var conSoft = new ContractSoftware()
        {
            ContractFK = contract.ContractId,
            SoftwareSystemFK = software.SoftwareId,
            PriceInContract = 2000.01M,
            UpdateTime = 2,
            Version = "versioncon"
        };

        await _context.ContractSoftwares.AddAsync(conSoft);
        await _context.SaveChangesAsync();

        var pay = new Payment()
        {
            ClientFK = client.ClientId,
            ContractFK = contract.ContractId,
            ValuePaid = 2000.01M
        };

        await _context.Payments.AddAsync(pay);
        await _context.SaveChangesAsync();

        
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = client.ClientId,
                Firma_czy_Klient = "Klient"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = software2.SoftwareId,
                    UpdateTime = 1
                }
            }
        };
        
            await _contractService.CreateContract(toAdd, new CancellationToken());


        var numOfRes = await _context.Contracts.CountAsync();

        var resNum = await _context.Contracts
            .FirstOrDefaultAsync(x => x.StartDate == toAdd.StartDate);

        var resNumOfContractSoft = await _context.ContractSoftwares
            .FirstOrDefaultAsync(x => x.ContractFK == resNum.ContractId);

        var resOfPayment = await _context.Payments
            .FirstOrDefaultAsync(x => x.ContractFK == resNum.ContractId);
        
        
        
        Assert.That(numOfRes, Is.EqualTo(2));
        Assert.That(resNum.Price, Is.EqualTo(3800.0095M));
        Assert.That(resNumOfContractSoft.PriceInContract, Is.EqualTo(3800.0095M));
        Assert.That(resOfPayment.ValuePaid, Is.EqualTo(0));
    }

    [Fact]
    public async Task Create_Contract_Can_Be_Added_With_Discounted_Price()
    {
        var client = new Client()
        {
            Email = "user@wp.pl",
            Surname = "testSur",
            PESEL = "12345678900",
            PhoneNumber = "123456789",
            Name = "test"
        };

        await _context.Clients.AddAsync(client);
        await _context.SaveChangesAsync();

        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 1000.01M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();

        var disc1 = new Discount()
        {
            DateStart = DateTime.Now,
            DateEnd = DateTime.Now.AddDays(2),
            Name = "Test1",
            Offer = "Offer1",
            Value = 10,
            SoftwareSystems = new List<SoftwareSystem>()
            {
                software
            }
        };

        await _context.Discounts.AddAsync(disc1);
        await _context.SaveChangesAsync();
        
        var disc2 = new Discount()
        {
            DateStart = DateTime.Now,
            DateEnd = DateTime.Now.AddDays(2),
            Name = "Test2",
            Offer = "Offer2",
            Value = 20,
            SoftwareSystems = new List<SoftwareSystem>()
            {
                software
            }
        };
        
        await _context.Discounts.AddAsync(disc2);
        await _context.SaveChangesAsync();
        
        ContractDTO toAdd = new ContractDTO()
        {
            ClientInfo = new PaymentAddDTO()
            {
                ClientId = client.ClientId,
                Firma_czy_Klient = "Klient"
            },
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(12),
            Softwares = new List<ContractSoftwareDTO>()
            {
                new ContractSoftwareDTO()
                {
                    SoftwareSystemFK = software.SoftwareId,
                    UpdateTime = 1
                }
            }
        };
        
        await _contractService.CreateContract(toAdd, new CancellationToken());


        var numOfRes = await _context.Contracts.CountAsync();

        var resNum = await _context.Contracts
            .FirstOrDefaultAsync(x => x.StartDate == toAdd.StartDate);

        var resNumOfContractSoft = await _context.ContractSoftwares
            .FirstOrDefaultAsync(x => x.ContractFK == resNum.ContractId);

        var resOfPayment = await _context.Payments
            .FirstOrDefaultAsync(x => x.ContractFK == resNum.ContractId);
        
        
        
        Assert.That(numOfRes, Is.EqualTo(1));
        Assert.That(resNum.Price, Is.EqualTo(800.008M));
        Assert.That(resNumOfContractSoft.PriceInContract, Is.EqualTo(800.008M));
        Assert.That(resOfPayment.ValuePaid, Is.EqualTo(0));
        
    }
    
}