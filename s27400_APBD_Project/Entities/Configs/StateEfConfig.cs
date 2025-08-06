using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace s27400_APBD_Project.Entities.Configs;

public class StateEfConfig : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.HasKey(x => x.StateId).HasName("StateId");
        builder.Property(x => x.StateId).UseIdentityColumn();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(30);

        builder.ToTable("State");
    }
}