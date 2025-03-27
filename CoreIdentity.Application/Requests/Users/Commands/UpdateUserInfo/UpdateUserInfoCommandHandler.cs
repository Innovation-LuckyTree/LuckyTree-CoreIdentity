using CoreIdentity.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreIdentity.Application.Requests.Users.Queries.UpdateUserInfo;

public class UpdateUserInfoCommandHandler : IRequestHandler<UpdateUserInfoCommand, Unit>
{
    private readonly ICoreIdentityDbContext _dbContext;

    public UpdateUserInfoCommandHandler(ICoreIdentityDbContext dbContext) => _dbContext = dbContext;

    public async Task<Unit> Handle(UpdateUserInfoCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.Where(o => o.Id == request.UserId).FirstOrDefaultAsync(cancellationToken);
        _ = user ?? throw new Exception($"Unable to find User with UserID: {request.UserId}");

        user.Email = request.Email;
        user.MobileNumber = request.MobileNumber;
        user.UserName = request.MobileNumber;

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}