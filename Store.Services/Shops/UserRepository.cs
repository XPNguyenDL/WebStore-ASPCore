using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Data.Contexts;

namespace Store.Services.Shops;

public class UserRepository : IUserRepository
{
	private readonly StoreDbContext _dbContext;

	public UserRepository(StoreDbContext context)
	{
		_dbContext = context;
	}

	public async Task<User> GetUser(string username, string password, CancellationToken cancellationToken = default)
	{
		return await _dbContext.Users.FirstOrDefaultAsync(user =>
			user.Username.Equals(username) && user.Password.Equals(password));
	}

	public Task<User> AddOrUpdatePostAsync(User post, IEnumerable<string> tags, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}