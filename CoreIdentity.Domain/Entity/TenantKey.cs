using CoreIdentity.Domain.Common;

namespace CoreIdentity.Domain.Entity
{
    public class TenantKey : AuditableEntity
    {
        public Guid TenantKeyId { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public string Key { get; set; }
        public string Salt { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}