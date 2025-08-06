using Microsoft.EntityFrameworkCore;
using s27400_APBD_Project.Entities;
using s27400_APBD_Project.PaymentPart.DTOs;

namespace s27400_APBD_Project.PaymentPart.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly SoftwareDbContext _context;

    public PaymentRepository(SoftwareDbContext context)
    {
        _context = context;
    }
    

    public async Task<int> IfContractExists(int contractId, CancellationToken token)
    {
        var res = await _context.Contracts
            .FirstOrDefaultAsync(x => x.ContractId == contractId, token);

        if (res == null)
        {
            return 0;
        }

        return res.ContractId;
    }

    public async Task<int> VerifyContractState(int contractId, CancellationToken token)
    {
        var res = await _context.Contracts
            .FirstOrDefaultAsync(x => x.ContractId == contractId, token);

        if (res.EndDate > DateTime.Now && res.StateFK == 1)
        {
            return 0;
        }

        if (res.StateFK == 2)
        {
            return 2;
        }

        if (res.StateFK == 3)
        {
            return 3;
        }
        
        return 1;
    }

    public async Task<decimal> HowMuchMoneyIsAlreadyPaid(int contractId, CancellationToken token)
    {
        var res = await _context.Payments
            .Where(x => x.ContractFK == contractId)
            .Select(x => x.ValuePaid).ToListAsync(token);

        decimal sum = res.Sum();

        return sum;

    }

    public async Task<decimal> GetContractCost(int contractId, CancellationToken token)
    {
        var res = await _context.Contracts
            .FirstOrDefaultAsync(x => x.ContractId == contractId, token);

        return res.Price;
    }

    public bool ValidatePayment(PaymentDTO payment, decimal alreadyPaid, decimal contractCost)
    {
        decimal leftToBePaid = contractCost - alreadyPaid;

        if (leftToBePaid - payment.PaymentValue < 0)
        {
            return false;
        }

        return true;
    }

    public bool VerifyIfPaymentIsMoreThan0(PaymentDTO payment)
    {
        if (payment.PaymentValue > 0)
        {
            return true;
        }

        return false;
    }


    public async Task<bool> PayForContract(PaymentDTO payment, CancellationToken token)
    {
        var firstPayment = await _context.Payments
            .FirstOrDefaultAsync(x => x.ContractFK == payment.ContractId, token);

        Payment p = new Payment()
        {
            ClientFK = firstPayment.ClientFK,
            CompanyFK = firstPayment.CompanyFK,
            ContractFK = firstPayment.ContractFK,
            ValuePaid = payment.PaymentValue
        };

        await _context.Payments.AddAsync(p, token);
        await _context.SaveChangesAsync(token);

        return true;
    }

    public async Task<bool> ChangeContractStatus(int contractId, int statusCode, CancellationToken token)
    {
        var res = await _context.Contracts
            .FirstOrDefaultAsync(x => x.ContractId == contractId, token);

        res.StateFK = statusCode;

        await _context.SaveChangesAsync(token);
        return true;
    }

    public async Task<string> TransactionPacked(bool toUpdate, PaymentDTO payment, CancellationToken token)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync(token))
        {
            try
            {
                await PayForContract(payment, token);
                if (toUpdate)
                {
                    await ChangeContractStatus(payment.ContractId, 2, token);
                }

                await transaction.CommitAsync(token);

                return "Płatność zaksięgowana pomyślnie";

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(token);
                throw new Exception($"404 Niespodziwany błąd podczas realizowania transakcji");
            }
        }
    }
}