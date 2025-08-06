using s27400_APBD_Project.PaymentPart.DTOs;

namespace s27400_APBD_Project.PaymentPart.Services;

public interface IPaymentService
{
    public Task<string> CreatePayment(PaymentDTO payment, CancellationToken token);
}