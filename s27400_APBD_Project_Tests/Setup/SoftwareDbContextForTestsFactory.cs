using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using s27400_APBD_Project.Entities;

namespace s27400_APBD_Project_Tests.Setup;

public class SoftwareDbContextForTestsFactory
{
    public static SoftwareDbContext CreateInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<SoftwareDbContext>()
            .UseInMemoryDatabase("TestDB")
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var dbContext = new SoftwareDbContext(options);
        

        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}