using CoreIdentity.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoreIdentity.Application.Requests.Users.Queries.GetLastActivity
{
    public record GetLastActivityQuery(Guid UserId) : IRequest<DateTimeOffset>;
    public class GetLastActivityQueryHandler(ICoreIdentityDbContext dbContext) : IRequestHandler<GetLastActivityQuery, DateTimeOffset>
    {
        private readonly ICoreIdentityDbContext _dbContext = dbContext;

        public async Task<DateTimeOffset> Handle(GetLastActivityQuery request, CancellationToken cancellationToken)
        {
            var logdata = await _dbContext.UserLogs.Where(o => o.UserId == request.UserId)
                .OrderByDescending(m => m.LoginDate).FirstOrDefaultAsync();

            if (logdata == null)
                throw new Exception("No login record!");

            return logdata.LoginDate.ToUniversalTime();
        }
    }
}
