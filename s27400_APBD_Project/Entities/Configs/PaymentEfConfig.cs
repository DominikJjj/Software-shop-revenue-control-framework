using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace s27400_APBD_Project.Entities.Configs;

public class PaymentEfConfig : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(x => x.PaymentId).HasName("PaymentId");
        builder.Property(x => x.PaymentId).UseIdentityColumn();

        builder.Property(x => x.ValuePaid).IsRequired().HasPrecision(10, 2);
        
        builder.HasOne(x => x.ClientNavigation)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.ClientFK)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.CompanyNavigation)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.CompanyFK)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ContractNavigation)
            .WithMany(x => x.Payments)
            .HasForeignKey(x => x.ContractFK)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("Payment");
        
    }
}