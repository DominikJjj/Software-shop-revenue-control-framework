using s27400_APBD_Project.PaymentPart.DTOs;
using s27400_APBD_Project.PaymentPart.Repositories;

namespace s27400_APBD_Project.PaymentPart.Services;

public class PaymentService : IPaymentService
{

    private readonly IPaymentRepository _paymentRepository;

    public PaymentService(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    
    public async Task<string> CreatePayment(PaymentDTO payment, CancellationToken token)
    {
        int contractId = await _paymentRepository.IfContractExists(payment.ContractId, token);

        if (contractId == 0)
        {
            throw new Exception($"400 Nieoczekiwany błąd kontrakt o id {payment.ContractId} nie istnieje w systemie");
        }

        int stateVerification = await _paymentRepository.VerifyContractState(contractId, token);

        if (stateVerification == 1)
        {
            await _paymentRepository.ChangeContractStatus(contractId, 3, token);
            throw new Exception("400 Kontrakt został anulowany");
        }

        if (stateVerification == 2)
        {
            throw new Exception("400 Kontrakt jest już opłacony");
        }

        if (stateVerification == 3)
        {
            throw new Exception("400 Kontrak jest anulowanym kontraktem");
        }

        bool valueIsZeroOrBelow = _paymentRepository.VerifyIfPaymentIsMoreThan0(payment);

        if (valueIsZeroOrBelow == false)
        {
            throw new Exception("400 Płacona kwota nie może być mniejsza lub równa 0");
        }

        decimal alreadyPaid = await _paymentRepository.HowMuchMoneyIsAlreadyPaid(contractId, token);
        decimal totalCost = await _paymentRepository.GetContractCost(contractId, token);

        bool paymentValidation = _paymentRepository.ValidatePayment(payment, alreadyPaid, totalCost);

        if (paymentValidation == false)
        {
            throw new Exception("400 Płacona kwota przewyższa wartość pozostałą do zapłacenia");
        }

        bool toUpdate = false;

        if (alreadyPaid + payment.PaymentValue == totalCost)
        {
            toUpdate = true;
        }

        return await _paymentRepository.TransactionPacked(toUpdate, payment, token);

    }

}