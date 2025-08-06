using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace s27400_APBD_Project.Entities.Configs;

public class DiscountEfConfig : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(x => x.DiscountId).HasName("DiscountId");
        builder.Property(x => x.DiscountId).UseIdentityColumn();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Offer).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Value).IsRequired();
        builder.Property(x => x.DateStart).IsRequired();
        builder.Property(x => x.DateEnd).IsRequired();

        builder.ToTable("Discount");
    }
}