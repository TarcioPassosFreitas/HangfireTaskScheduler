using Ardalis.Result;
using HangfireTaskScheduler.Core.Aggregate.UserAggregate;

namespace HangfireTaskScheduler.Core.Interfaces.Repository;

public interface IUserRepository
{
    Task<User> AddAsync(User user, CancellationToken cancellationToken);
}