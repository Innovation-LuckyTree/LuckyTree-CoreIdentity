using CoreIdentity.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreIdentity.Application.Notifications.LoginUser;

public class LoginAttemptNotificationHandler : INotificationHandler<LoginAttemptNotification>
{
    private readonly ICoreIdentityDbContext _dbContext;
    private const int _maxAttempts = 5;

    public LoginAttemptNotificationHandler(ICoreIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(LoginAttemptNotification notification, CancellationToken cancellationToken)
    {
        var attempts = notification.Attempts > 0 ? notification.Attempts + 1 : 1;

        var user = await _dbContext.Users.Where(o => o.Id == notification.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
            return;

        if (attempts >= _maxAttempts)
        {
            user.Locked = true;
            user.LockTime = DateTimeOffset.UtcNow;
        }

        user.Attempts = attempts;

        _dbContext.Users.Update(user);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}