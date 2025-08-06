using s27400_APBD_Project.PaymentPart.DTOs;

namespace s27400_APBD_Project.PaymentPart.Repositories;

public interface IPaymentRepository
{
    public Task<int> IfContractExists(int contractId, CancellationToken token);
    public Task<int> VerifyContractState(int contractId, CancellationToken token);
    public Task<decimal> HowMuchMoneyIsAlreadyPaid(int contractId, CancellationToken token);
    public Task<decimal> GetContractCost(int contractId, CancellationToken token);
    public bool ValidatePayment(PaymentDTO payment, decimal alreadyPaid, decimal contractCost);
    public bool VerifyIfPaymentIsMoreThan0(PaymentDTO payment);
    public Task<bool> PayForContract(PaymentDTO payment, CancellationToken token);
    public Task<bool> ChangeContractStatus(int contractId, int statusCode, CancellationToken token);
    public Task<string> TransactionPacked(bool toUpdate, PaymentDTO payment, CancellationToken token);
}