namespace CoreIdentity.Domain.Common
{
    public class AuditableEntity
    {
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? LastModifiedBy { get; set; }
    }
}