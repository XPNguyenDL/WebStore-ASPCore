using Store.Core.Entities;

namespace Store.Services.Shops;

public interface IUserRepository
{
	Task<User> GetUser(string username, string password, CancellationToken cancellationToken = default);

	Task<User> AddOrUpdatePostAsync(User post, IEnumerable<string> tags,
		CancellationToken cancellationToken = default);


}