using Microsoft.EntityFrameworkCore;
using s27400_APBD_Project.ContractPart.DTOs;
using s27400_APBD_Project.Entities;

namespace s27400_APBD_Project.ContractPart.Repositories;

public class ContractRepository : IContractReposiotry
{
    private readonly SoftwareDbContext _context;

    public ContractRepository(SoftwareDbContext context)
    {
        _context = context;
    }

    public bool ValidateDates(DateTime start, DateTime end)
    {
        if ((end.Date - start.Date).Days > 30)
        {
            return false;
        }

        if ((end.Date - start.Date).Days < 3)
        {
            return false;
        }
        return true;
    }

    public async Task<bool> ValidateOwnership(ContractDTO contract, CancellationToken token)
    {
        List<int> ContractIds = new List<int>();
        if (contract.ClientInfo.Firma_czy_Klient == "Klient")
        {
            ContractIds = await _context.Payments
                .Where(x => x.ClientFK == contract.ClientInfo.ClientId)
                .Select(x => x.ContractFK)
                .ToListAsync(token);
        }

        if (contract.ClientInfo.Firma_czy_Klient == "Firma")
        {
            ContractIds = await _context.Payments
                .Where(x => x.CompanyFK == contract.ClientInfo.ClientId)
                .Select(x => x.ContractFK)
                .ToListAsync(token);
        }

        if (ContractIds.Count == 0)
        {
            return true;
        }

        List<int> SoftwareIds = new List<int>();

        foreach (var temp in contract.Softwares)
        {
            SoftwareIds.Add(temp.SoftwareSystemFK);
        }

        var query = _context.Contracts
            .Where(x => ContractIds.Contains(x.ContractId) && x.StateFK == 2)
            .Include(x => x.ContractSoftwares);

        var res = await query
            .Select(x => new UpdateLenghtVerificationDTO()
            {
                DateTill = x.EndDate,
                IdsWithTime = x.ContractSoftwares.Select(k => new SoftwareTimeDTO()
                {
                    AdditionalYears = k.UpdateTime,
                    Id = k.SoftwareSystemFK
                }).ToList()
            }).ToListAsync(token);

        foreach (int softId in SoftwareIds)
        {
            DateTime max = new DateTime(2000, 12, 12);
            foreach (UpdateLenghtVerificationDTO verify in res)
            {
                foreach (SoftwareTimeDTO timeId in verify.IdsWithTime)
                {
                    if (timeId.Id == softId)
                    {
                        if (max < verify.DateTill.AddYears(timeId.AdditionalYears))
                        {
                            max = verify.DateTill.AddYears(timeId.AdditionalYears);
                        }
                    }
                }
            }

            if (max > contract.EndDate)
            {
                return false;
            }
        }
        
        return true;
    }

    public bool ValidateList(List<ContractSoftwareDTO> softwareList)
    {
        if (softwareList.Count > 0)
        {
            return true;
        }

        return false;
    }

    public async Task<bool> ValidateClient(PaymentAddDTO dto, CancellationToken token)
    {
        if (dto.Firma_czy_Klient == "Klient")
        {
            var res = await _context
                .Clients.FirstOrDefaultAsync(x => x.ClientId == dto.ClientId, token);

            if (res == null)
            {
                return false;
            }

            return true;
        }

        if (dto.Firma_czy_Klient == "Firma")
        {
            var res = await _context
                .Companies.FirstOrDefaultAsync(x => x.CompanyId == dto.ClientId, token);

            if (res == null)
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public async Task<bool> ValidateSoftware(List<ContractSoftwareDTO> softwareList, CancellationToken token)
    {
        List<int> ids = new List<int>();

        foreach (var temp in softwareList)
        {
            ids.Add(temp.SoftwareSystemFK);
        }

        var res = await _context.SoftwareSystems
            .Where(x => ids.Contains(x.SoftwareId))
            .ToListAsync(token);

        if (res.Count == ids.Count)
        {
            return true;
        }

        return false;
    }

    public async Task<List<NewFixPriceDTO>> CalculatePriceWithoutAdditionalUpdate(List<ContractSoftwareDTO> softwareList, CancellationToken token)
    {
        List<int> ids = new List<int>();
        foreach (var temp in softwareList)
        {
            ids.Add(temp.SoftwareSystemFK);
        }
        
        var query = _context.SoftwareSystems
            .Include(x => x.Discounts)
            .Where(x => ids.Contains(x.SoftwareId));
        

        var res = await query
            .Select(x => new
            {
                SoftwareId = x.SoftwareId,
                BasePrice = x.Price,
                discountsPercents = x.Discounts
                    .Where(d => d.DateStart <= DateTime.Now && d.DateEnd >= DateTime.Now)
                    .Select(d => d.Value)
                    .ToList()
            }).ToListAsync(token);

        List<NewFixPriceDTO> result = new List<NewFixPriceDTO>();

        foreach (var temp in res)
        {
            decimal disc = 0;
            if (temp.discountsPercents.Count != 0)
            {
                disc = temp.discountsPercents.Max();
            }
            
            result.Add(new NewFixPriceDTO()
            {
                Price = (temp.BasePrice - (temp.BasePrice * (disc / 100M))),
                SoftwareId = temp.SoftwareId
            });
        }

        return result;
    }

    public decimal CalculateAdditionalUpdatesPrice(List<ContractSoftwareDTO> softwareList, List<NewFixPriceDTO> fix)
    {
        int sum = 0;
        foreach (var temp in softwareList)
        {
            sum = sum + temp.UpdateTime - 1;
            foreach (var inFix in fix)
            {
                if (temp.SoftwareSystemFK == inFix.SoftwareId)
                {
                    inFix.Price = inFix.Price + ((temp.UpdateTime - 1) * 1000M);
                }
            }
        }

        return sum * 1000M;
    }

    public async Task<decimal> CalculateRegularCustomerDiscount(string type, int id, decimal price, CancellationToken token)
    {
        if (type == "Klient")
        {
            var res = await _context.Payments
                .Where(x => x.ClientFK == id && x.ContractNavigation.StateFK == 2)
                .Select(x => x.PaymentId)
                .ToListAsync(token);

            if (res.Count() == 0)
            {
                return 0;
            }
            else
            {
                return ((price / 100) * 5);
            }
        }

        if (type == "Firma")
        {
            var res = await _context.Payments
                .Where(x => x.CompanyFK == id && x.ContractNavigation.StateFK == 2)
                .Select(x => x.PaymentId)
                .ToListAsync(token);
            
            if (res.Count() == 0)
            {
                return 0;
            }
            else
            {
                return ((price / 100) * 5);
            }
        }

        return -1;
    }

    public async Task<int> AddContract(ContractDTO contract, CancellationToken token, decimal price)
    {
        Contract c = new Contract()
        {
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            StateFK = 1,
            Price = price
        };

        await _context.Contracts.AddAsync(c, token);
        await _context.SaveChangesAsync(token);

        return c.ContractId;
    }

    public async Task<bool> AddPayemnt(ContractDTO contract, int contractId, CancellationToken token)
    {
        if (contract.ClientInfo.Firma_czy_Klient == "Klient")
        {
            Payment payment = new Payment()
            {
                ClientFK = contract.ClientInfo.ClientId,
                ContractFK = contractId,
                ValuePaid = 0
            };

            await _context.Payments.AddAsync(payment, token);
            await _context.SaveChangesAsync(token);

            return true;
        }

        if (contract.ClientInfo.Firma_czy_Klient == "Firma")
        {
            Payment payment = new Payment()
            {
                CompanyFK = contract.ClientInfo.ClientId,
                ContractFK = contractId,
                ValuePaid = 0
            };
            
            await _context.Payments.AddAsync(payment, token);
            await _context.SaveChangesAsync(token);

            return true;
            
        }

        return false;
    }

    public async Task<List<ContractSoftware>> GetValuesToAddContractSoftware(ContractDTO contract, int contractId, CancellationToken token, List<NewFixPriceDTO> fix)
    {
        List<ContractSoftware> all = new List<ContractSoftware>();
        List<int> ids = new List<int>();

        foreach (var temp in contract.Softwares)
        {
            ids.Add(temp.SoftwareSystemFK);
        }

        var versions = await _context.SoftwareSystems
            .Where(x => ids.Contains(x.SoftwareId))
            .Select(x => new
            {
                id = x.SoftwareId,
                ver = x.Version
            })
            .ToListAsync(token);
        
        string tempVer = "";


            foreach (var temp in contract.Softwares)
            {
                decimal partPrice = 0;
                foreach (var soft in fix)
                {
                    if (soft.SoftwareId == temp.SoftwareSystemFK)
                    {
                        partPrice = soft.Price;
                    }
                }
                foreach (var iter in versions)
                {
                    if (iter.id == temp.SoftwareSystemFK)
                    {
                        tempVer = iter.ver;
                        break;
                    }
                }
                all.Add(new ContractSoftware()
                {
                    ContractFK = contractId,
                    SoftwareSystemFK = temp.SoftwareSystemFK,
                    UpdateTime = temp.UpdateTime,
                    Version = tempVer,
                    PriceInContract = partPrice
                });
            }
        
        return all;
    }

    public async Task<bool> AddAllContractSoftware(List<ContractSoftware> all, CancellationToken token)
    {
        foreach (var temp in all)
        {
            await _context.ContractSoftwares.AddAsync(temp, token);

        }
        await _context.SaveChangesAsync(token);
        return true;
    }
    
    public async Task<string> AddingPackedInTransaction(ContractDTO contract, CancellationToken token, decimal price, List<NewFixPriceDTO> fix)
    {
        using (var transaction = await _context.Database.BeginTransactionAsync(token))
        {
            try
            {
                int newContractId = await AddContract(contract, token, price);
                bool paymentVerifiation = await AddPayemnt(contract, newContractId, token);

                if (paymentVerifiation == false)
                {
                    throw new Exception("404 Nieoczekiwany błąd związany z dodaniem rekordu płatności do umowy");
                }

                List<ContractSoftware> all =
                    await GetValuesToAddContractSoftware(contract, newContractId, token, fix);

                await AddAllContractSoftware(all, token);
                await transaction.CommitAsync(token);
                return
                    $"Dodano kontrakt o id: {newContractId} wraz ze wszystkimi informacjami o płatności oraz programach";

            }
            catch (Exception e)
            {
                await transaction.RollbackAsync(token);
                throw new Exception($"404 Niespodziwany błąd podczas realizowania transakcji");
            }
        }
    }
}