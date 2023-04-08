using Store.Core.Entities;

namespace Store.Services.Shops;

public interface IUserRepository
{
	Task<User> GetUser(string username, string password, CancellationToken cancellationToken = default);

	Task<User> AddOrUpdateUserAsync(User user, IEnumerable<string> roles,
		CancellationToken cancellationToken = default);


}