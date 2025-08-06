using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using s27400_APBD_Project.Entities.Configs;

namespace s27400_APBD_Project.Entities;

public class SoftwareDbContext : DbContext
{
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Company> Companies { get; set; }
    public virtual DbSet<Contract> Contracts { get; set; }
    public virtual DbSet<ContractSoftware> ContractSoftwares { get; set; }
    public virtual DbSet<Discount> Discounts { get; set; }
    public virtual DbSet<Payment> Payments { get; set; }
    public virtual DbSet<SoftwareCategory> SoftwareCategories { get; set; }
    public virtual DbSet<SoftwareSystem> SoftwareSystems { get; set; }
    public virtual DbSet<State> States { get; set; }


    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<User> Users { get; set; }

    public SoftwareDbContext()
    {
    }

    public SoftwareDbContext(DbContextOptions<SoftwareDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ClientEfConfig).Assembly);

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.SqlServer")
        {
            modelBuilder.Entity<Company>().HasData(
                new Company()
                {
                    CompanyId = 1, Name = "TechSolutions Sp. z o.o.", Address = "ul. Miodowa 10, 00-251 Warszawa",
                    Email = "kontakt@techsolutions.pl", PhoneNumber = "221234567", KRS = "0000456789"
                },
                new Company()
                {
                    CompanyId = 2, Name = "GreenEnergy Polska S.A.", Address = "ul. Zielona 15, 30-001 Kraków",
                    Email = "info@greenenergy.pl", PhoneNumber = "123456789", KRS = "0000567890"
                },
                new Company()
                {
                    CompanyId = 3, Name = "WebDesign Experts Sp. z o.o.", Address = "ul. Kwiatowa 22, 60-101 Poznań",
                    Email = "biuro@webdesignexperts.pl", PhoneNumber = "612345678", KRS = "0000678901"
                },
                new Company()
                {
                    CompanyId = 4, Name = "SmartLogistics S.A.", Address = "ul. Transportowa 5, 40-002 Katowice",
                    Email = "office@smartlogistics.pl", PhoneNumber = "324567890", KRS = "0000789012"
                }
            );

            modelBuilder.Entity<Client>().HasData(
                new Client()
                {
                    ClientId = 1, Name = "Anna", Surname = "Kowalska", Email = "anna.kowalska@example.com",
                    PhoneNumber = "600123456", PESEL = "84051412345"
                },
                new Client()
                {
                    ClientId = 2, Name = "Piotr", Surname = "Nowak", Email = "piotr.nowak@example.com",
                    PhoneNumber = "601234567", PESEL = "92071823456"
                },
                new Client()
                {
                    ClientId = 3, Name = "Katarzyna", Surname = "Wiśniewska",
                    Email = "katarzyna.wisniewska@example.com",
                    PhoneNumber = "602345678", PESEL = "78090234567"
                },
                new Client()
                {
                    ClientId = 4, Name = "Marek", Surname = "Zieliński", Email = "marek.zielinski@example.com",
                    PhoneNumber = "603456789", PESEL = "86031945678"
                });

            modelBuilder.Entity<Contract>().HasData(
                new Contract()
                {
                    ContractId = 1, StartDate = new DateTime(2022, 12, 12), EndDate = new DateTime(2022, 12, 29),
                    Price = 8000.01M, StateFK = 2
                },
                new Contract()
                {
                    ContractId = 2, StartDate = new DateTime(2022, 12, 12), EndDate = new DateTime(2022, 12, 17),
                    Price = 5000.11M, StateFK = 3
                },
                new Contract()
                {
                    ContractId = 3, StartDate = new DateTime(2022, 11, 11), EndDate = new DateTime(2022, 11, 28),
                    Price = 9000.11M, StateFK = 2
                },
                new Contract()
                {
                    ContractId = 4, StartDate = new DateTime(2023, 4, 4), EndDate = new DateTime(2023, 4, 22),
                    Price = 4000.01M, StateFK = 3
                });

            modelBuilder.Entity<ContractSoftware>().HasData(
                new ContractSoftware()
                {
                    ContractSoftwareId = 1, ContractFK = 1, SoftwareSystemFK = 1, Version = "1.0", UpdateTime = 1,
                    PriceInContract = 4000.01M
                },
                new ContractSoftware()
                {
                    ContractSoftwareId = 2, ContractFK = 1, SoftwareSystemFK = 2, Version = "build 2.0", UpdateTime = 1,
                    PriceInContract = 4000.00M
                },
                new ContractSoftware()
                {
                    ContractSoftwareId = 3, ContractFK = 2, SoftwareSystemFK = 1, Version = "1.0", UpdateTime = 1,
                    PriceInContract = 4000.01M
                },
                new ContractSoftware()
                {
                    ContractSoftwareId = 4, ContractFK = 2, SoftwareSystemFK = 3, Version = "2.21", UpdateTime = 1,
                    PriceInContract = 1000.10M
                },
                new ContractSoftware()
                {
                    ContractSoftwareId = 5, ContractFK = 3, SoftwareSystemFK = 1, Version = "1.1", UpdateTime = 1,
                    PriceInContract = 4000.01M
                },
                new ContractSoftware()
                {
                    ContractSoftwareId = 6, ContractFK = 3, SoftwareSystemFK = 2, Version = "build 2.1", UpdateTime = 1,
                    PriceInContract = 4000.00M
                },
                new ContractSoftware()
                {
                    ContractSoftwareId = 7, ContractFK = 3, SoftwareSystemFK = 3, Version = "2.73", UpdateTime = 1,
                    PriceInContract = 1000.10M
                },
                new ContractSoftware()
                {
                    ContractSoftwareId = 8, ContractFK = 4, SoftwareSystemFK = 1, Version = "1.1", UpdateTime = 1,
                    PriceInContract = 4000.01M
                });

            modelBuilder.Entity<Discount>().HasData(
                new Discount()
                {
                    DiscountId = 1, Name = "Wakacje 2023", Offer = "Wszystko", Value = 15,
                    DateStart = new DateTime(2023, 06, 21), DateEnd = new DateTime(2023, 7, 7)
                },
                new Discount()
                {
                    DiscountId = 2, Name = "Wakacje 2024", Offer = "Wszystko", Value = 10,
                    DateStart = new DateTime(2024, 06, 21), DateEnd = new DateTime(2024, 7, 7)
                },
                new Discount()
                {
                    DiscountId = 3, Name = "Black Friday 2024", Offer = "Wszystko", Value = 20,
                    DateStart = new DateTime(2024, 11, 29), DateEnd = new DateTime(2024, 12, 1)
                }
                );

            modelBuilder.Entity<Payment>().HasData(new Payment()
                {
                    PaymentId = 1, ClientFK = 1, ContractFK = 1, ValuePaid = 8000.01M
                },
                new Payment()
                {
                    PaymentId = 2, ClientFK = 3, ContractFK = 2, ValuePaid = 100.00M
                },
                new Payment()
                {
                    PaymentId = 3, CompanyFK = 1, ContractFK = 3, ValuePaid = 9000.11M
                },
                new Payment()
                {
                    PaymentId = 4, CompanyFK = 2, ContractFK = 4, ValuePaid = 400.01M
                });

            modelBuilder.Entity<Role>().HasData(
                new Role()
                {
                    RoleId = 1, Name = "Standard"
                },
                new Role()
                {
                    RoleId = 2, Name = "Admin"
                });

            modelBuilder.Entity<SoftwareCategory>().HasData(
                new SoftwareCategory()
                {
                    CategoryId = 1, Name = "Edukacja"
                },
                new SoftwareCategory()
                {
                    CategoryId = 2, Name = "Finanse"
                },
                new SoftwareCategory()
                {
                    CategoryId = 3, Name = "Rozrywka"
                });

            modelBuilder.Entity<SoftwareSystem>().HasData(
                new SoftwareSystem()
                {
                    SoftwareId = 1, Name = "Rachunki", Description = "Description Rachunki", Version = "1.9",
                    Price = 4000.01M, CategoryFK = 2
                },
                new SoftwareSystem()
                {
                    SoftwareId = 2, Name = "Nauczanie", Description = "Description Nauczanie", Version = "build 2.20",
                    Price = 4000.00M, CategoryFK = 1
                },
                new SoftwareSystem()
                {
                    SoftwareId = 3, Name = "Filmy", Description = "Description Filmy", Version = "2.92",
                    Price = 1000.10M, CategoryFK = 3
                });

            modelBuilder.Entity<State>().HasData(
                new State()
                {
                    StateId = 1, Name = "Oczekujace"
                },
                new State()
                {
                    StateId = 2, Name = "Oplacone"
                },
                new State()
                {
                    StateId = 3, Name = "Anulowane"
                });
            modelBuilder.Entity<Discount>()
                .HasMany(x => x.SoftwareSystems)
                .WithMany(x => x.Discounts)
                .UsingEntity<Dictionary<object, string>>("ProductDiscount",
                    j => j.HasOne<SoftwareSystem>().WithMany().HasPrincipalKey("SoftwareId"),
                    j => j.HasOne<Discount>().WithMany().HasForeignKey("DiscountId"))
                .HasData(
                    new { SoftwareSystemsSoftwareId = 1, DiscountId = 1 },
                    new { SoftwareSystemsSoftwareId = 2, DiscountId = 1 },
                    new { SoftwareSystemsSoftwareId = 3, DiscountId = 1 },
                    new {SoftwareSystemsSoftwareId = 1, DiscountId = 2},
                    new {SoftwareSystemsSoftwareId = 2, DiscountId = 2 },
                    new {SoftwareSystemsSoftwareId = 3, DiscountId = 2 },
                    new {SoftwareSystemsSoftwareId = 1, DiscountId = 3 },
                    new {SoftwareSystemsSoftwareId = 2, DiscountId = 3 },
                    new {SoftwareSystemsSoftwareId = 3, DiscountId = 3 }
                );
        }
        

    }
}