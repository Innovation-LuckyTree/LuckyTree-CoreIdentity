using CoreIdentity.Domain.Common;

namespace CoreIdentity.Domain.Entity;

public class UserDeviceToken : AuditableEntity
{
    public Guid UserDeviceTokenId { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public string Key { get; set; }
    public string Salt { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public string DeviceName { get; set; }
    public string DeviceModel { get; set; }

    public virtual User User { get; set; }
}
