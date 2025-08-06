using s27400_APBD_Project.IncomePart.Repositories;

namespace s27400_APBD_Project.IncomePart.Services;

public class IncomeService : IIncomeService
{
    private readonly IIncomeRepository _incomeRepository;

    public IncomeService(IIncomeRepository incomeRepository)
    {
        _incomeRepository = incomeRepository;
    }

    public async Task<string> GetIncomeCompany(string code, CancellationToken token)
    {
        decimal income = await _incomeRepository.GetCompanyIncome(token);
        decimal newIncome = await _incomeRepository.ConvertCurrency(income, code, token);
        return $"Dochód całej firmy wynosi {newIncome} " + code + ".";
    }

    public async Task<string> GetEstimatedIncomeCompany(string code, CancellationToken token)
    {
        decimal income = await _incomeRepository.GetEstimatedCompanyIncome(token);
        decimal newIncome = await _incomeRepository.ConvertCurrency(income, code, token);
        return $"Prognozowany dochód całej firmy wynosi {newIncome} " + code + ".";
    }


    public async Task<string> GetIncomeProduct(string code, int id, CancellationToken token)
    {
        bool verify = await _incomeRepository.VerifyProductExisting(id, token);

        if (verify == false)
        {
            throw new Exception($"400 Program o id {id} nie istnieje w bazie");
        }

        decimal income = await _incomeRepository.GetProductIncome(id, token);
        decimal newIncome = await _incomeRepository.ConvertCurrency(income, code, token);
        return $"Przychód dla programu o id {id} wynosi {newIncome} " + code + ".";
    }

    public async Task<string> GetEstimatedIncomeProduct(string code, int id, CancellationToken token)
    {
        bool verify = await _incomeRepository.VerifyProductExisting(id, token);

        if (verify == false)
        {
            throw new Exception($"400 Program o id {id} nie istnieje w bazie");
        }

        decimal income = await _incomeRepository.GetEstimantedProductIncome(id, token);
        decimal newIncome = await _incomeRepository.ConvertCurrency(income, code, token);
        return $"Prognozowany przychód dla programu o id {id} wynosi {newIncome} " + code + ".";
    }
    
}