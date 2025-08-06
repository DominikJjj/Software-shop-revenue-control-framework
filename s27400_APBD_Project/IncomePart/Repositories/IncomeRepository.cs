using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using s27400_APBD_Project.Entities;

namespace s27400_APBD_Project.IncomePart.Repositories;

public class IncomeRepository : IIncomeRepository
{
    private readonly SoftwareDbContext _context;

    public IncomeRepository(SoftwareDbContext context)
    {
        _context = context;
    }

    public async Task<bool> VerifyProductExisting(int id, CancellationToken token)
    {
        var res = await _context.SoftwareSystems
            .FirstOrDefaultAsync(x => x.SoftwareId == id, token);

        if (res == null)
        {
            return false;
        }

        return true;
    }

    public async Task<decimal> GetCompanyIncome(CancellationToken token)
    {
        var res = await _context.Contracts
            .Where(x => x.StateFK == 2)
            .Select(x => x.Price)
            .ToListAsync(token);

        return res.Sum();
    }

    public async Task<decimal> GetEstimatedCompanyIncome(CancellationToken token)
    {
        var res = await _context.Contracts
            .Where(x => x.StateFK == 2 || x.StateFK == 1)
            .Select(x => x.Price)
            .ToListAsync(token);

        return res.Sum();
    }

    public async Task<decimal> GetProductIncome(int id, CancellationToken token)
    {
        var res = await _context.ContractSoftwares
            .Where(x => x.SoftwareSystemFK == id && x.ContractNavigation.StateFK == 2)
            .Select(x => x.PriceInContract)
            .ToListAsync(token);

        return res.Sum();
    }

    public async Task<decimal> GetEstimantedProductIncome(int id, CancellationToken token)
    {
        var res = await _context.ContractSoftwares
            .Where(x => x.SoftwareSystemFK == id &&
                        (x.ContractNavigation.StateFK == 2 || x.ContractNavigation.StateFK == 1))
            .Select(x => x.PriceInContract)
            .ToListAsync(token);

        return res.Sum();
    }

    public async Task<decimal> ConvertCurrency(decimal price, string code, CancellationToken token)
    {
        using (HttpClient client = new HttpClient())
        {
            var httpResponseMessage = await client.GetAsync("https://open.er-api.com/v6/latest/PLN", token);
            httpResponseMessage.EnsureSuccessStatusCode();
            var read = await httpResponseMessage.Content.ReadAsStringAsync(token);

            var json = JObject.Parse(read);

            var rate = json["rates"][code];

            if (rate == null)
            {
                throw new Exception($"400 {code} waluta nie istnieje");
            }

            return Math.Round(((decimal)rate * price), 2);
        }
    }
}