using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace s27400_APBD_Project.Entities.Configs;

public class SoftwareCategoryEfConfig : IEntityTypeConfiguration<SoftwareCategory>
{
    public void Configure(EntityTypeBuilder<SoftwareCategory> builder)
    {
        builder.HasKey(x => x.CategoryId).HasName("CategoryId");
        builder.Property(x => x.CategoryId).UseIdentityColumn();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(30);

        builder.ToTable("SoftwareCategory");
    }
}