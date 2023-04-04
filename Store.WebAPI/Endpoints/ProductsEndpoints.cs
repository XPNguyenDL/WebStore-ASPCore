using Mapster;
using MapsterMapper;
using Store.Core.Collections;
using Store.Core.Contracts;
using Store.Core.Entities;
using Store.Services.Shops;
using Store.WebAPI.Media;
using Store.WebAPI.Models;
using Store.WebAPI.Models.ProductModel;
using System.Net;

namespace Store.WebAPI.Endpoints;

public static class ProductsEndpoints
{
	public static WebApplication MapProductEndpoints(
		this WebApplication app)
	{
		var routeGroupBuilder = app.MapGroup("/api/products");

		routeGroupBuilder.MapGet("/", GetProducts)
			.WithName("GetProducts")
			.Produces<IPagedList<ProductDto>>();

		routeGroupBuilder.MapGet("/{id:guid}", GetProductById)
			.WithName("GetProductById")
			.Produces<ProductDto>();

		routeGroupBuilder.MapPost("/{id:guid}/picture", SetProductPicture)
			.WithName("SetProductPicture")
			.Accepts<IFormFile>("multipart/form-data")
			.Produces<ApiResponse<string>>()
			.Produces(400);

		return app;
	}

	private static async Task<IResult> GetProductById(
		Guid id,
		ICollectionRepository repository,
		IMapper mapper)
	{
		var product = await repository.GetProductById(id);
		
		if (product == null)
		{
			return Results.NotFound("Không tìm thấy sản phẩm");
		}

		var productDetail = mapper.Map<ProductDto>(product);

		return Results.Ok(productDetail);
	}

	private static async Task<IResult> GetProducts(
		[AsParameters] ProductFilterModel model,
		ICollectionRepository repository,
		IMapper mapper)
	{
		var condition = mapper.Map<ProductQuery>(model);

		var products =
			await repository.GetPagedProductsAsync(
				condition,
				model,
				p => p.ProjectToType<ProductDto>());

		var paginationResult = new PaginationResult<ProductDto>(products);

		return Results.Ok(paginationResult);
	}

	private static async Task<IResult> SetProductPicture(
		Guid id,
		IFormFile imageFile,
		ICollectionRepository repository,
		IMediaManager mediaManager)
	{
		var oldProduct = await repository.GetProductById(id);
		if (oldProduct == null)
		{
			return Results.Ok(ApiResponse.Fail(
				HttpStatusCode.NotFound,
				$"Không tìm thấy sản phẩm với id: `{id}`"));
		}
		

		var imageUrl = await mediaManager.SaveFileAsync(
			imageFile.OpenReadStream(),
			imageFile.FileName, imageFile.ContentType);
		if (string.IsNullOrWhiteSpace(imageUrl))
		{
			return Results.Ok(ApiResponse.Fail(
				HttpStatusCode.BadRequest,
				"Không lưu được tệp"));
		}
		await repository.SetImageUrlAsync(id, imageUrl);
		return Results.Ok(ApiResponse.Success("Lưu thành công"));
	}
	
}