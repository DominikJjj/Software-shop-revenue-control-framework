using s27400_APBD_Project.ContractPart.DTOs;
using s27400_APBD_Project.ContractPart.Repositories;
using s27400_APBD_Project.Entities;

namespace s27400_APBD_Project.ContractPart.Services;

public class ContractService : IContractService
{
    private readonly IContractReposiotry _contractReposiotry;

    public ContractService(IContractReposiotry contractReposiotry)
    {
        _contractReposiotry = contractReposiotry;
    }

    public async Task<string> CreateContract(ContractDTO contractData, CancellationToken token)
    {
        bool datesVerification = _contractReposiotry.ValidateDates(contractData.StartDate, contractData.EndDate);

        if (datesVerification == false)
        {
            throw new Exception("400 Przedział czasowy powinien wynosić między 3 a 30 dni.");
        }

        bool listVerification = _contractReposiotry.ValidateList(contractData.Softwares);

        if (listVerification == false)
        {
            throw new Exception("400 W skład kontraktu muszą wchodzić programy.");
        }

        bool clientVerification = await _contractReposiotry.ValidateClient(contractData.ClientInfo, token);

        if (clientVerification == false)
        {
            throw new Exception(
                $"400 {contractData.ClientInfo.Firma_czy_Klient} o id: {contractData.ClientInfo.ClientId} nie istnieje w systemie");
        }

        bool softwareVerification = await _contractReposiotry.ValidateSoftware(contractData.Softwares, token);

        if (softwareVerification == false)
        {
            throw new Exception("400 Podany program nie wystepuje w bazie");
        }

        bool ownershipVerification = await _contractReposiotry.ValidateOwnership(contractData, token);

        if (ownershipVerification == false)
        {
            throw new Exception($"400 Zamawiający posiada juz aktywne licencje na wybrane z zamawianych programów");
        }
        
        List<NewFixPriceDTO> temp = await _contractReposiotry.CalculatePriceWithoutAdditionalUpdate(contractData.Softwares, token);

        decimal price = 0;

        foreach (var soft in temp)
        {
            price = price + soft.Price;
        }
            
        
        price = price + _contractReposiotry.CalculateAdditionalUpdatesPrice(contractData.Softwares, temp);

        decimal regularDiscount =
            await _contractReposiotry.CalculateRegularCustomerDiscount(contractData.ClientInfo.Firma_czy_Klient,
                contractData.ClientInfo.ClientId, price, token);

        if (regularDiscount > 0)
        {
            foreach (var soft in temp)
            {
                soft.Price = soft.Price * 0.95M;
            }
        }

        if (regularDiscount == -1)
        {
            throw new Exception("404 Błędy rodzaj klienta");
        }

        price = price - regularDiscount;

        return await _contractReposiotry.AddingPackedInTransaction(contractData, token, price, temp);

    }
}