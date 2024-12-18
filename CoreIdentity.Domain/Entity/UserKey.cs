using CoreIdentity.Domain.Common;

namespace CoreIdentity.Domain.Entity
{
    public class UserKey : AuditableEntity
    {
        public int UserKeyId { get; set; }
        public Guid UserId { get; set; }
        public string Key { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset ExpirationDate { get; set; }

        public User User { get; set; }
    }
}