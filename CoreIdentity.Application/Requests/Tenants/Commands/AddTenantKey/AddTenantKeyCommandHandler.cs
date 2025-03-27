using System.Data;
using CoreIdentity.Application.Common.Interfaces;
using CoreIdentity.Domain.Entity;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static CoreIdentity.Application.Common.Extensions.CryptographyExtensions;

namespace CoreIdentity.Application.Requests.Tenants.Commands.AddTenantKey;

public class AddTenantKeyCommandHandler : IRequestHandler<AddTenantKeyCommand, TenantKeyResult>
{
    private readonly ICoreIdentityDbContext _dbContext;

    public AddTenantKeyCommandHandler(ICoreIdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TenantKeyResult> Handle(AddTenantKeyCommand request, CancellationToken cancellationToken)
    {
        var tenant = await _dbContext.Tenants
            .Where(o => o.Id == request.TenantId)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        _ = tenant ?? throw new Exception($"Unable to find tenant ID {request.TenantId}");

        var key = CreateKey();
        var keyHash = CreatePassword(key);

        var tenantKey = new TenantKey
        {
            TenantId = request.TenantId,
            Key = keyHash.Password,
            Salt = keyHash.Salt,
            StartDate = request.StartDate.ToUniversalTime(),
            ExpirationDate = request.ExpirationDate.ToUniversalTime()
        };

        _dbContext.TenantKeys.Add(tenantKey);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TenantKeyResult(tenantKey.TenantKeyId, tenant.Id, key, "Bearer");
    }
}