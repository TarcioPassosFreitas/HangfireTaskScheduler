using Ardalis.Result;
using HangfireTaskScheduler.Core.Aggregate.UserAggregate;

namespace HangfireTaskScheduler.Core.Interfaces.Service;

public interface IUserService
{
    Task<Result<User>> AddAsync(User user, CancellationToken cancellationToken);
}