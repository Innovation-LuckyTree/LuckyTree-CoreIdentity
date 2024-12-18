using CoreIdentity.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreIdentity.Persistence.Configurations;

public class RolesConfiguration : IEntityTypeConfiguration<Roles>
{
    public void Configure(EntityTypeBuilder<Roles> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.RoleName)
            .HasMaxLength(50);

        builder.Property(e => e.CreatedOn).IsRequired();

        // Data seeder
        builder.HasData(
            new Roles { Id = 1, RoleName = "Super Admin", CreatedOn = DateTimeOffset.UtcNow }, // 1
            new Roles { Id = 2, RoleName = "BDM", CreatedOn = DateTimeOffset.UtcNow }, // 2
            new Roles { Id = 3, RoleName = "BDO", CreatedOn = DateTimeOffset.UtcNow } // 2
        );
    }
}
