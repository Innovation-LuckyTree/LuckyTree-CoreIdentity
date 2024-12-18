using CoreIdentity.Application.Common.Interfaces;
using CoreIdentity.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using CoreIdentity.Application.Common.Extensions;
using CoreIdentity.Application.Notifications.LoginUser;
using CoreIdentity.Application.Requests.Users.Queries.CreateUserJwtToken;

namespace CoreIdentity.Application.Requests.Users.Queries.GetUserToken;

public class GetUserTokenQueryHandler : IRequestHandler<GetUserTokenQuery, UserTokenDto>
{
    private readonly ILogger<GetUserTokenQueryHandler> _logger;
    private readonly ICoreIdentityDbContext _dbContext;
    private readonly IAppConfig _appConfig;
    private readonly IMediator _mediator;
    private readonly DateTime _expiryDateTime;

    public GetUserTokenQueryHandler(ILogger<GetUserTokenQueryHandler> logger, ICoreIdentityDbContext dbContext,
        IAppConfig appConfig, IMediator mediator)
    {
        _logger = logger;
        _dbContext = dbContext;
        _appConfig = appConfig;
        _mediator = mediator;
        _expiryDateTime = DateTime.UtcNow.AddHours(_appConfig.TokenExpiryHours);
    }

    public async Task<UserTokenDto> Handle(GetUserTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.Where(o => o.UserName.Equals(request.UserName))
            .Include(o => o.TenantUsers)
            .Include(o => o.UserRoles)
                .ThenInclude(e => e.Roles)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return null;
        }

        if (user.Locked)
        {
            var stillLocked = user.LockTime.Value.AddMinutes(_appConfig.LockTimeMinutes) > DateTimeOffset.UtcNow;

            if (stillLocked)
            {
                return new UserTokenDto
                {
                    IsLocked = true
                };
            }            
        }

        if (!await ValidateUser(request, user))
        {
            return null;
        }

        var refreshToken = CryptographyExtensions.CreateKey();
        var refreshTokenExpiration = _expiryDateTime.AddMinutes(30).ToUniversalTime();

        await _mediator.Publish(new LoginUserNotification(user.Id, refreshToken, refreshTokenExpiration, request.TenantId, request.IpAddress), cancellationToken);

        return await _mediator.Send(new CreateUserJwtTokenQuery(user, request.TenantId, refreshToken), cancellationToken);
    }

    private async Task<bool> ValidateUser(GetUserTokenQuery request, User user)
    {
        if (string.IsNullOrEmpty(request.TenantId))
        {
            var isTenantUser = user.TenantUsers
                .Any(o => o.TenantId.ToString() == request.TenantId);

            if (!isTenantUser)
            {
                return false;
            }
        }

        if (request.Password.GetPasswordHash(user.PasswordSalt) != user.Password)
        {
            await _mediator.Publish(new LoginAttemptNotification(user.Id, user.Attempts));

            return false;
        }

        return true;
    }
}
