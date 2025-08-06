using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace s27400_APBD_Project.Entities.Configs;

public class ContractEfConfig : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(x => x.ContractId).HasName("ContractId");
        builder.Property(x => x.ContractId).UseIdentityColumn();

        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();
        builder.Property(x => x.Price).HasPrecision(10, 2);

        builder.HasOne(x => x.StateNavigation)
            .WithMany(x => x.Contracts)
            .HasForeignKey(x => x.StateFK)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Contract");

    }
}