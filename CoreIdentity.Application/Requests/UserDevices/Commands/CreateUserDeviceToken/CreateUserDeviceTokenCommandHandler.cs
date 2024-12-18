using CoreIdentity.Application.Common.Exceptions;
using CoreIdentity.Application.Common.Interfaces;
using CoreIdentity.Domain.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static CoreIdentity.Application.Common.Extensions.CryptographyExtensions;

namespace CoreIdentity.Application.Requests.UserDevices.Commands.GetUserDeviceToken;

public class CreateUserDeviceTokenCommandHandler : IRequestHandler<CreateUserDeviceTokenCommand, UserDeviceTokenDto>
{
    public readonly ICoreIdentityDbContext _coreIdentityDbContext;

    public CreateUserDeviceTokenCommandHandler(ICoreIdentityDbContext coreIdentityDbContext)
    {
        _coreIdentityDbContext = coreIdentityDbContext;
    }

    public async Task<UserDeviceTokenDto> Handle(CreateUserDeviceTokenCommand request, CancellationToken cancellationToken)
    {
        var user = await _coreIdentityDbContext.Users.Where(o => o.Id == request.UserId)
            .FirstOrDefaultAsync(cancellationToken);

        _ = user ?? throw new EntityNotFoundException("User", request.UserId);

        var key = CreateKey();
        var keyHash = CreatePassword(key);

        var userDeviceToken = new UserDeviceToken
        {
            UserId = user.Id,
            Key = keyHash.Password,
            Salt = keyHash.Salt,
            StartDate = DateTimeOffset.UtcNow,
            ExpirationDate = DateTimeOffset.UtcNow.AddDays(360),
            DeviceName = request.DeviceName,
            DeviceModel = request.DeviceModel
        };

        _coreIdentityDbContext.UserDeviceTokens.Add(userDeviceToken);

        await _coreIdentityDbContext.SaveChangesAsync(cancellationToken);

        return new UserDeviceTokenDto
        {
            DeviceTokenId = userDeviceToken.UserDeviceTokenId,
            Key = key,
            DeviceModel = request.DeviceModel,
            DeviceName = request.DeviceName
        };
    }
}
