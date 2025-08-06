using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace s27400_APBD_Project.Entities.Configs;

public class ContractSoftwareEfConfig : IEntityTypeConfiguration<ContractSoftware>
{
    public void Configure(EntityTypeBuilder<ContractSoftware> builder)
    {
        builder.HasKey(x => x.ContractSoftwareId).HasName("ContractSoftwareId");
        builder.Property(x => x.ContractSoftwareId).UseIdentityColumn();

        builder.Property(x => x.UpdateTime).IsRequired();
        builder.Property(x => x.Version).IsRequired().HasMaxLength(20);
        builder.Property(x => x.PriceInContract).IsRequired().HasPrecision(7, 2);

        builder.HasOne(x => x.ContractNavigation)
            .WithMany(x => x.ContractSoftwares)
            .HasForeignKey(x => x.ContractFK)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.SoftwareSystemNavigation)
            .WithMany(x => x.ContractSoftwares)
            .HasForeignKey(x => x.SoftwareSystemFK)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("ContractSoftware");
    }
}