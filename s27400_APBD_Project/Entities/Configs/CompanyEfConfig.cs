using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace s27400_APBD_Project.Entities.Configs;

public class CompanyEfConfig : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.HasKey(x => x.CompanyId).HasName("CompanyId");
        builder.Property(x => x.CompanyId).UseIdentityColumn();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(60);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(60);
        builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(9);
        builder.Property(x => x.KRS).IsRequired().HasMaxLength(10);

        builder.ToTable("Company");

        
    }
}