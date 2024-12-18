using CoreIdentity.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreIdentity.Persistence.Configurations
{
    public class TenantKeyConfiguration : IEntityTypeConfiguration<TenantKey>
    {
        public void Configure(EntityTypeBuilder<TenantKey> builder)
        {
            builder.ToTable("TenantKey");

            builder.HasKey(o => o.TenantKeyId);

            builder.Property(o => o.TenantKeyId);

            builder.Property(o => o.Key)
                .HasMaxLength(100);

            builder.Property(o => o.Salt)
                .HasMaxLength(150);

            builder.HasOne(e => e.Tenant)
                .WithMany(f => f.TenantKeys)
                .HasForeignKey(e => e.TenantId);
        }
    }
}