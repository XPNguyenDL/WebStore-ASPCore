using Microsoft.EntityFrameworkCore;
using Store.Core.Entities;
using Store.Core.Identity;
using Store.Data.Contexts;

namespace Store.Services.Shops;

public class UserRepository : IUserRepository
{
	private readonly StoreDbContext _dbContext;
	private readonly IPasswordHasher _hasher;

	public UserRepository(StoreDbContext context, IPasswordHasher hasher)
	{
		_dbContext = context;
		_hasher = hasher;
	}

	public async Task<User> GetUser(string username, string password, CancellationToken cancellationToken = default)
	{
		var user = await _dbContext.Set<User>()
			.Include(s => s.Roles)
			.FirstOrDefaultAsync(user =>
				user.Username.Equals(username), cancellationToken);

		if (user != null && _hasher.VerifyPassword(user.Password, password))
		{
			return user;
		}
		return null;
	}

	public Task<User> AddOrUpdateUserAsync(User user, IEnumerable<string> roles, CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}