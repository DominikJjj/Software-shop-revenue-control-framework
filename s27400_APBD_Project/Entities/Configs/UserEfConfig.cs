using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace s27400_APBD_Project.Entities.Configs;

public class UserEfConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.UserId).HasName("UserId");
        builder.Property(x => x.UserId).UseIdentityColumn();

        builder.Property(x => x.Login).IsRequired().HasMaxLength(30);
        builder.Property(x => x.Password).IsRequired();
        builder.Property(x => x.Salt).IsRequired();
        builder.Property(x => x.RefreshToken).IsRequired();
        builder.Property(x => x.DueDateRefreshToken).IsRequired();

        builder.HasOne(x => x.RoleNavigation)
            .WithMany(x => x.Users)
            .HasForeignKey(x => x.RoleFK)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ToTable("User");
    }
}