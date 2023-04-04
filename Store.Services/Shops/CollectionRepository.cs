using Microsoft.EntityFrameworkCore;
using Store.Core.Contracts;
using Store.Core.Entities;
using Store.Data.Contexts;
using Store.Services.Extensions;

namespace Store.Services.Shops;

public class CollectionRepository : ICollectionRepository
{
	private readonly StoreDbContext _dbContext;

	public CollectionRepository(StoreDbContext context)
	{
		_dbContext = context;
	}


	public async Task<Product> GetProductById(Guid id, CancellationToken cancellationToken = default)
	{
		return await _dbContext.Set<Product>()
			.Include(s => s.Category)
			.Include(s => s.Pictures)
			.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
	}

	public async Task<Product> GetProductBySlug(string slug, CancellationToken cancellationToken = default)
	{
		return await _dbContext.Set<Product>()
			.Include(s => s.Category)
			.Include(s => s.Pictures)
			.FirstOrDefaultAsync(s => s.UrlSlug.Equals(slug), cancellationToken);
	}

	public Task<IPagedList<Product>> GetPagedProductsAsync(IProductQuery productQuery, IPagingParams pagingParams,
		CancellationToken cancellationToken = default)
	{
		return FilterProduct(productQuery).ToPagedListAsync(pagingParams, cancellationToken);
	}

	public async Task<IPagedList<T>> GetPagedProductsAsync<T>(IProductQuery condition, IPagingParams pagingParams, Func<IQueryable<Product>, IQueryable<T>> mapper)
	{
		var products = FilterProduct(condition);
		var projectedProducts = mapper(products);

		return await projectedProducts.ToPagedListAsync(pagingParams);
	}

	public async Task<bool> SetImageUrlAsync(Guid productId, string imageUrl, CancellationToken cancellationToken = default)
	{
		if (_dbContext.Set<Product>().FirstOrDefault(s => s.Id == productId) == null)
		{
			return false;
		}
		
		var picture = new Picture()
		{
			Id = Guid.NewGuid(),
			ProductId = productId,
			Path = imageUrl,
			Active = true
		};

		_dbContext.Pictures.Add(picture);
		await _dbContext.SaveChangesAsync(cancellationToken);
		return true;
	}

	private IQueryable<Product> FilterProduct(IProductQuery condition)
	{
		var products = _dbContext.Set<Product>()
			.Include(s => s.Category)
			.Include(s => s.Pictures)
			.WhereIf(condition.Year > 0, s => s.CreateDate.Year == condition.Year)
			.WhereIf(condition.Month > 0, s => s.CreateDate.Month == condition.Month)
			.WhereIf(condition.Day > 0, s => s.CreateDate.Day == condition.Day)
			.WhereIf(!string.IsNullOrEmpty(condition.CategorySlug), s => s.Category.UrlSlug.Contains(condition.CategorySlug))
			.WhereIf(!string.IsNullOrEmpty(condition.ProductSlug), s => s.UrlSlug.Contains(condition.ProductSlug))
			.WhereIf(!string.IsNullOrEmpty(condition.Keyword), s =>
				s.Name.Contains(condition.Keyword) ||
				s.Description.Contains(condition.Keyword) ||
				s.ShortIntro.Contains(condition.Keyword) ||
				s.UrlSlug.Contains(condition.Keyword));
		return products;
	}
}