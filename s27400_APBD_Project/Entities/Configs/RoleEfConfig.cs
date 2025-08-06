using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace s27400_APBD_Project.Entities.Configs;

public class RoleEfConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.RoleId).HasName("RoleId");
        builder.Property(x => x.RoleId).UseIdentityColumn();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(30);

        builder.ToTable("Role");
        
    }
}