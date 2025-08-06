using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace s27400_APBD_Project.Entities.Configs;

public class SoftwareSystemEfConfig : IEntityTypeConfiguration<SoftwareSystem>
{
    public void Configure(EntityTypeBuilder<SoftwareSystem> builder)
    {
        builder.HasKey(x => x.SoftwareId).HasName("SoftwareId");
        builder.Property(x => x.SoftwareId).UseIdentityColumn();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(60);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(150);
        builder.Property(x => x.Version).IsRequired().HasMaxLength(20);
        builder.Property(x => x.Price).IsRequired().HasPrecision(6, 2);

        builder.HasOne(x => x.SoftwareCategoryNavigation)
            .WithMany(x => x.SoftwareSystems)
            .HasForeignKey(x => x.CategoryFK)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("SoftwareSystem");
        
    }
}