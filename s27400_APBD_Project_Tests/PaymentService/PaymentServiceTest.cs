using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using s27400_APBD_Project_Tests.Setup;
using s27400_APBD_Project.Entities;
using s27400_APBD_Project.PaymentPart.DTOs;
using Assert = NUnit.Framework.Assert;

namespace s27400_APBD_Project_Tests.PaymentService;

public class PaymentServiceTest
{
    private readonly SoftwareDbContext _context;
    private readonly s27400_APBD_Project.PaymentPart.Services.PaymentService _paymentService;

    public PaymentServiceTest()
    {
        _context = SoftwareDbContextForTestsFactory.CreateInMemoryDbContext();
        _paymentService = new s27400_APBD_Project.PaymentPart.Services.PaymentService(
            new s27400_APBD_Project.PaymentPart.Repositories.PaymentRepository(_context));
    }
    
    //createPayment()
    [Fact]
    public async Task Create_Payment_Should_Not_Create_Payment_Because_No_Contract_With_This_Id_In_Database()
    {
        var contract = new Contract()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(4),
            Price = 1000.01M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var payemntDto = new PaymentDTO()
        {
            ContractId = contract.ContractId + 1,
            PaymentValue = 10.10M
        };
        

        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _paymentService.CreatePayment(payemntDto, new CancellationToken()));
        
        Assert.That(res.Message, Does.Contain("nie istnieje w systemie"));
    }
    
    [Fact]
    public async Task Create_Payment_Should_Not_Create_Payment_Because_Contract_Is_Cancelled()
    {
        var contract = new Contract()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(4),
            Price = 1000.01M,
            StateFK = 3
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var payemntDto = new PaymentDTO()
        {
            ContractId = contract.ContractId,
            PaymentValue = 10.10M
        };
        

        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _paymentService.CreatePayment(payemntDto, new CancellationToken()));
        
        Assert.That(res.Message, Does.Contain("Kontrak jest anulowanym kontraktem"));
    }
    
    [Fact]
    public async Task Create_Payment_Should_Not_Create_Payment_Because_Contract_Is_Outdated()
    {
        var contract = new Contract()
        {
            StartDate = new DateTime(2022,12,12),
            EndDate = new DateTime(2022,12,22),
            Price = 1000.01M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var payemntDto = new PaymentDTO()
        {
            ContractId = contract.ContractId,
            PaymentValue = 10.10M
        };
        

        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _paymentService.CreatePayment(payemntDto, new CancellationToken()));
        
        Assert.That(contract.StateFK, Is.EqualTo(3));
        
        Assert.That(res.Message, Does.Contain("Kontrakt został anulowany"));
    }
    
    [Fact]
    public async Task Create_Payment_Should_Not_Create_Payment_Because_Contract_Is_Paid()
    {
        var contract = new Contract()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(4),
            Price = 1000.01M,
            StateFK = 2
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var payemntDto = new PaymentDTO()
        {
            ContractId = contract.ContractId,
            PaymentValue = 10.10M
        };
        

        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _paymentService.CreatePayment(payemntDto, new CancellationToken()));
        
        
        Assert.That(res.Message, Does.Contain("Kontrakt jest już opłacony"));
    }
    
    [Fact]
    public async Task Create_Payment_Should_Not_Create_Payment_Because_Value_In_Payment_Is_Zero()
    {
        var contract = new Contract()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(4),
            Price = 1000.01M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var payemntDto = new PaymentDTO()
        {
            ContractId = contract.ContractId,
            PaymentValue = 0M
        };
        

        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _paymentService.CreatePayment(payemntDto, new CancellationToken()));
        
        
        Assert.That(res.Message, Does.Contain("Płacona kwota nie może być mniejsza lub równa 0"));
    }
    
    [Fact]
    public async Task Create_Payment_Should_Not_Create_Payment_Because_Value_In_Payment_Is_Below_Zero()
    {
        var contract = new Contract()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(4),
            Price = 1000.01M,
            StateFK = 1
        };

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var payemntDto = new PaymentDTO()
        {
            ContractId = contract.ContractId,
            PaymentValue = -1.0M
        };
        

        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _paymentService.CreatePayment(payemntDto, new CancellationToken()));
        
        
        Assert.That(res.Message, Does.Contain("Płacona kwota nie może być mniejsza lub równa 0"));
    }
    
    [Fact]
    public async Task Create_Payment_Should_Not_Create_Payment_Because_Contract_Was_Already_Paid()
    {
        var contract = new Contract()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(4),
            Price = 1000.01M,
            StateFK = 1
        };
        

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var payment = new Payment()
        {
            ContractFK = contract.ContractId,
            CompanyFK = 1,
            ValuePaid = 1000.01M
        };

        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        var payemntDto = new PaymentDTO()
        {
            ContractId = contract.ContractId,
            PaymentValue = 10.0M
        };
        

        var res = Assert.ThrowsAsync<Exception>(async () =>
            await _paymentService.CreatePayment(payemntDto, new CancellationToken()));
        
        
        Assert.That(res.Message, Does.Contain("Płacona kwota przewyższa wartość pozostałą do zapłacenia"));
    }
    
    [Fact]
    public async Task Create_Payment_Should_Create_Payment_Without_Changing_Status_To_Paid()
    {
        var contract = new Contract()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(4),
            Price = 1000.01M,
            StateFK = 1
        };
        

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var payment = new Payment()
        {
            ContractFK = contract.ContractId,
            CompanyFK = 1,
            ValuePaid = 900.01M
        };

        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        var payemntDto = new PaymentDTO()
        {
            ContractId = contract.ContractId,
            PaymentValue = 10.0M
        };

        await _paymentService.CreatePayment(payemntDto, new CancellationToken());

        var NumOfPaymnets = await _context.Payments.CountAsync();
        
        Assert.That(NumOfPaymnets, Is.EqualTo(2));
        Assert.That(contract.StateFK, Is.EqualTo(1));
    }
    
    
    [Fact]
    public async Task Create_Payment_Should_Create_Payment_With_Changing_Status_To_Paid()
    {
        var contract = new Contract()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(4),
            Price = 1000.01M,
            StateFK = 1
        };
        

        await _context.Contracts.AddAsync(contract);
        await _context.SaveChangesAsync();

        var payment = new Payment()
        {
            ContractFK = contract.ContractId,
            CompanyFK = 1,
            ValuePaid = 900.01M
        };

        await _context.Payments.AddAsync(payment);
        await _context.SaveChangesAsync();

        var payemntDto = new PaymentDTO()
        {
            ContractId = contract.ContractId,
            PaymentValue = 100.0M
        };

        await _paymentService.CreatePayment(payemntDto, new CancellationToken());

        var NumOfPaymnets = await _context.Payments.CountAsync();
        
        Assert.That(NumOfPaymnets, Is.EqualTo(2));
        Assert.That(contract.StateFK, Is.EqualTo(2));
    }
}