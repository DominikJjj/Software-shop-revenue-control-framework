using NUnit.Framework;
using NUnit.Framework.Legacy;
using s27400_APBD_Project_Tests.Setup;
using s27400_APBD_Project.Entities;
using Assert = NUnit.Framework.Assert;

namespace s27400_APBD_Project_Tests.IncomeService;

public class IncomeServiceTest
{
    private readonly SoftwareDbContext _context;
    private readonly s27400_APBD_Project.IncomePart.Services.IncomeService _incomeService;

    public IncomeServiceTest()
    {
        _context = SoftwareDbContextForTestsFactory.CreateInMemoryDbContext();
        _incomeService =
            new s27400_APBD_Project.IncomePart.Services.IncomeService(
                new s27400_APBD_Project.IncomePart.Repositories.IncomeRepository(_context));
    }
    
    //GetIncomeCompany()
    
    [Fact]
    public async Task Get_Company_Income_Result_Check()
    {
        var contract = new Contract()
        {
            StartDate = new DateTime(2022, 12, 12),
            EndDate = new DateTime(2022, 12, 29),
            Price = 20000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        
        var contract2 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 10000.10M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract2);
        await _context.SaveChangesAsync();
        
        var contract3 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract3);
        await _context.SaveChangesAsync();

        string res = await _incomeService.GetIncomeCompany("PLN", new CancellationToken());
        
        StringAssert.Contains("54000,20 PLN", res);
    }
    
    
    [Fact]
    public async Task Get_Estimated_Company_Income_Result_Check()
    {
        var contract = new Contract()
        {
            StartDate = new DateTime(2022, 12, 12),
            EndDate = new DateTime(2022, 12, 29),
            Price = 20000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        
        var contract2 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 10000.10M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract2);
        await _context.SaveChangesAsync();
        
        var contract3 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract3);
        await _context.SaveChangesAsync();
        
        var contract4 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 3
        };

        await _context.Contracts.AddAsync(contract4);
        await _context.SaveChangesAsync();

        string res = await _incomeService.GetEstimatedIncomeCompany("PLN", new CancellationToken());

        Assert.That(res, Does.Contain("64000,30 PLN"));
    }
    
    [Fact]
    public async Task Get_Product_Income_Result_Check_Product_Not_exist()
    {
        
        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 34000.10M,
            Version = "version"
        };

        var software2 = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "decs1",
            Name = "Name12",
            Price = 20000.10M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();
        await _context.SoftwareSystems.AddAsync(software2);
        await _context.SaveChangesAsync();
        
        var contract = new Contract()
        {
            StartDate = new DateTime(2022, 12, 12),
            EndDate = new DateTime(2022, 12, 29),
            Price = 20000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        
        var contract2 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 20000.10M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract2);
        await _context.SaveChangesAsync();
        
        var contract3 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract3);
        await _context.SaveChangesAsync();
        
        var contract4 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 3
        };

        await _context.Contracts.AddAsync(contract4);
        await _context.SaveChangesAsync();

        var conSoft1 = new ContractSoftware()
        {
            ContractFK = contract.ContractId,
            PriceInContract = 20000.10M,
            SoftwareSystemFK = software2.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft2 = new ContractSoftware()
        {
            ContractFK = contract2.ContractId,
            PriceInContract = 20000.10M,
            SoftwareSystemFK = software2.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft3 = new ContractSoftware()
        {
            ContractFK = contract3.ContractId,
            PriceInContract = 34000.10M,
            SoftwareSystemFK = software.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft4 = new ContractSoftware()
        {
            ContractFK = contract4.ContractId,
            PriceInContract = 34000.10M,
            SoftwareSystemFK = software.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        await _context.AddAsync(conSoft1);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft2);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft3);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft4);
        await _context.SaveChangesAsync();

        var res = Assert.ThrowsAsync<Exception>( async () =>
        {
            await _incomeService.GetIncomeProduct("PLN", 3, new CancellationToken());
        });
        
        Assert.That(res.Message, Does.Contain("nie istnieje w bazie"));
    }
    
        [Fact]
    public async Task Get_Product_Income_Result_Check()
    {
        
        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 34000.10M,
            Version = "version"
        };

        var software2 = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "decs1",
            Name = "Name12",
            Price = 20000.10M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();
        await _context.SoftwareSystems.AddAsync(software2);
        await _context.SaveChangesAsync();
        
        var contract = new Contract()
        {
            StartDate = new DateTime(2022, 12, 12),
            EndDate = new DateTime(2022, 12, 29),
            Price = 20000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        
        var contract2 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 20000.10M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract2);
        await _context.SaveChangesAsync();
        
        var contract3 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract3);
        await _context.SaveChangesAsync();
        
        var contract4 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 3
        };

        await _context.Contracts.AddAsync(contract4);
        await _context.SaveChangesAsync();

        var conSoft1 = new ContractSoftware()
        {
            ContractFK = contract.ContractId,
            PriceInContract = 20000.10M,
            SoftwareSystemFK = software2.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft2 = new ContractSoftware()
        {
            ContractFK = contract2.ContractId,
            PriceInContract = 20000.10M,
            SoftwareSystemFK = software2.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft3 = new ContractSoftware()
        {
            ContractFK = contract3.ContractId,
            PriceInContract = 34000.10M,
            SoftwareSystemFK = software.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft4 = new ContractSoftware()
        {
            ContractFK = contract4.ContractId,
            PriceInContract = 34000.10M,
            SoftwareSystemFK = software.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };

        await _context.AddAsync(conSoft1);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft2);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft3);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft4);
        await _context.SaveChangesAsync();
            

        var res = await _incomeService.GetIncomeProduct("PLN", software.SoftwareId, new CancellationToken());

        Assert.That(res, Does.Contain("34000,10"));
    }
    
        [Fact]
    public async Task Get_Estimated_Product_Income_Result_Check_Product_Not_exist()
    {
        
        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 34000.10M,
            Version = "version"
        };

        var software2 = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "decs1",
            Name = "Name12",
            Price = 20000.10M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();
        await _context.SoftwareSystems.AddAsync(software2);
        await _context.SaveChangesAsync();
        
        var contract = new Contract()
        {
            StartDate = new DateTime(2022, 12, 12),
            EndDate = new DateTime(2022, 12, 29),
            Price = 20000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        
        var contract2 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 20000.10M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract2);
        await _context.SaveChangesAsync();
        
        var contract3 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract3);
        await _context.SaveChangesAsync();
        
        var contract4 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 3
        };

        await _context.Contracts.AddAsync(contract4);
        await _context.SaveChangesAsync();

        var conSoft1 = new ContractSoftware()
        {
            ContractFK = contract.ContractId,
            PriceInContract = 20000.10M,
            SoftwareSystemFK = software2.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft2 = new ContractSoftware()
        {
            ContractFK = contract2.ContractId,
            PriceInContract = 20000.10M,
            SoftwareSystemFK = software2.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft3 = new ContractSoftware()
        {
            ContractFK = contract3.ContractId,
            PriceInContract = 34000.10M,
            SoftwareSystemFK = software.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft4 = new ContractSoftware()
        {
            ContractFK = contract4.ContractId,
            PriceInContract = 34000.10M,
            SoftwareSystemFK = software.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        await _context.AddAsync(conSoft1);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft2);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft3);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft4);
        await _context.SaveChangesAsync();

        var res = Assert.ThrowsAsync<Exception>( async () =>
        {
            await _incomeService.GetEstimatedIncomeProduct("PLN", 3, new CancellationToken());
        });
        
        Assert.That(res.Message, Does.Contain("nie istnieje w bazie"));
    }
    
     [Fact]
    public async Task Get_Estimated_Product_Income_Result_Check()
    {
        
        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 34000.10M,
            Version = "version"
        };

        var software2 = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "decs1",
            Name = "Name12",
            Price = 20000.10M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();
        await _context.SoftwareSystems.AddAsync(software2);
        await _context.SaveChangesAsync();
        
        var contract = new Contract()
        {
            StartDate = new DateTime(2022, 12, 12),
            EndDate = new DateTime(2022, 12, 29),
            Price = 20000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        
        var contract2 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 20000.10M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract2);
        await _context.SaveChangesAsync();
        
        var contract3 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract3);
        await _context.SaveChangesAsync();
        
        var contract4 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 3
        };

        await _context.Contracts.AddAsync(contract4);
        await _context.SaveChangesAsync();

        var conSoft1 = new ContractSoftware()
        {
            ContractFK = contract.ContractId,
            PriceInContract = 20000.10M,
            SoftwareSystemFK = software2.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft2 = new ContractSoftware()
        {
            ContractFK = contract2.ContractId,
            PriceInContract = 20000.10M,
            SoftwareSystemFK = software2.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft3 = new ContractSoftware()
        {
            ContractFK = contract3.ContractId,
            PriceInContract = 34000.10M,
            SoftwareSystemFK = software.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft4 = new ContractSoftware()
        {
            ContractFK = contract4.ContractId,
            PriceInContract = 34000.10M,
            SoftwareSystemFK = software.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };

        await _context.AddAsync(conSoft1);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft2);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft3);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft4);
        await _context.SaveChangesAsync();
            

        var res = await _incomeService.GetEstimatedIncomeProduct("PLN", software2.SoftwareId, new CancellationToken());

        Assert.That(res, Does.Contain("40000,20"));
    }
    
        [Fact]
    public async Task Get_Product_Income_Result_Check_Product_Currency_Code_Not_Exist()
    {
        
        var software = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "desc",
            Name = "Name",
            Price = 34000.10M,
            Version = "version"
        };

        var software2 = new SoftwareSystem()
        {
            CategoryFK = 1,
            Description = "decs1",
            Name = "Name12",
            Price = 20000.10M,
            Version = "version"
        };

        await _context.SoftwareSystems.AddAsync(software);
        await _context.SaveChangesAsync();
        await _context.SoftwareSystems.AddAsync(software2);
        await _context.SaveChangesAsync();
        
        var contract = new Contract()
        {
            StartDate = new DateTime(2022, 12, 12),
            EndDate = new DateTime(2022, 12, 29),
            Price = 20000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();
        
        var contract2 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 20000.10M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract2);
        await _context.SaveChangesAsync();
        
        var contract3 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract3);
        await _context.SaveChangesAsync();
        
        var contract4 = new Contract()
        {
            StartDate = new DateTime(2023, 12, 12),
            EndDate = new DateTime(2023, 12, 29),
            Price = 34000.10M,
            StateFK = 3
        };

        await _context.Contracts.AddAsync(contract4);
        await _context.SaveChangesAsync();

        var conSoft1 = new ContractSoftware()
        {
            ContractFK = contract.ContractId,
            PriceInContract = 20000.10M,
            SoftwareSystemFK = software2.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft2 = new ContractSoftware()
        {
            ContractFK = contract2.ContractId,
            PriceInContract = 20000.10M,
            SoftwareSystemFK = software2.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft3 = new ContractSoftware()
        {
            ContractFK = contract3.ContractId,
            PriceInContract = 34000.10M,
            SoftwareSystemFK = software.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        var conSoft4 = new ContractSoftware()
        {
            ContractFK = contract4.ContractId,
            PriceInContract = 34000.10M,
            SoftwareSystemFK = software.SoftwareId,
            UpdateTime = 1,
            Version = "ver1"
        };
        
        await _context.AddAsync(conSoft1);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft2);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft3);
        await _context.SaveChangesAsync();
        await _context.AddAsync(conSoft4);
        await _context.SaveChangesAsync();

        var res = Assert.ThrowsAsync<Exception>( async () =>
        {
            await _incomeService.GetIncomeProduct("ADS", software2.SoftwareId, new CancellationToken());
        });
        
        Assert.That(res.Message, Does.Contain("waluta nie istnieje"));
    }
}