namespace s27400_APBD_Project.IncomePart.Repositories;

public interface IIncomeRepository
{
    public Task<bool> VerifyProductExisting(int id, CancellationToken token);
    public Task<decimal> GetCompanyIncome(CancellationToken token);
    public Task<decimal> GetEstimatedCompanyIncome(CancellationToken token);
    public Task<decimal> GetProductIncome(int id, CancellationToken token);
    public Task<decimal> GetEstimantedProductIncome(int id, CancellationToken token);
    public Task<decimal> ConvertCurrency(decimal price, string code, CancellationToken token);
}