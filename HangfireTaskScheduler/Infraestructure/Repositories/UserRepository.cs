using HangfireTaskScheduler.Core.Aggregate.UserAggregate;
using HangfireTaskScheduler.Core.Interfaces.Repository;
using HangfireTaskScheduler.Infraestructure.Context;

namespace HangfireTaskScheduler.Infraestructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _databaseContext;

    public UserRepository(AppDbContext dbContext)
    {
        _databaseContext = dbContext;
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        await _databaseContext.Users.AddAsync(user);

        await _databaseContext.SaveChangesAsync(cancellationToken);

        return user;
    }
}