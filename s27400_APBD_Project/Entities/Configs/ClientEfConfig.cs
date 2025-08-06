using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace s27400_APBD_Project.Entities.Configs;

public class ClientEfConfig : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasKey(x => x.ClientId).HasName("ClientId");
        builder.Property(x => x.ClientId).UseIdentityColumn();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(40);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(60);
        builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(9);
        builder.Property(x => x.IsDeleted).IsRequired();
        builder.Property(x => x.PESEL).IsRequired().HasMaxLength(11);

        builder.ToTable("Client");
    }
}