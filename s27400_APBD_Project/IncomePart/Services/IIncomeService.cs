namespace s27400_APBD_Project.IncomePart.Services;

public interface IIncomeService
{
    public Task<string> GetIncomeCompany(string code, CancellationToken token);
    public Task<string> GetEstimatedIncomeCompany(string code, CancellationToken token);
    public Task<string> GetIncomeProduct(string code, int id, CancellationToken token);
    public Task<string> GetEstimatedIncomeProduct(string code, int id, CancellationToken token);
}