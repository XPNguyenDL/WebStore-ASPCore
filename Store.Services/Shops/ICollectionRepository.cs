using Store.Core.Contracts;
using Store.Core.Entities;

namespace Store.Services.Shops;

public interface ICollectionRepository
{
	Task<Product> GetProductById(Guid id, CancellationToken cancellationToken = default);

	Task<IPagedList<Product>> GetPagedProductsAsync(IProductQuery productQuery, IPagingParams pagingParams,
		CancellationToken cancellationToken = default);

	Task<IPagedList<T>> GetPagedProductsAsync<T>(
		IProductQuery condition,
		IPagingParams pagingParams,
		Func<IQueryable<Product>, IQueryable<T>> mapper);

	Task<bool> SetImageUrlAsync(Guid productId, string imageUrl, CancellationToken cancellationToken = default);
}