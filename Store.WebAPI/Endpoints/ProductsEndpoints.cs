namespace Store.WebAPI.Endpoints;

public static class ProductsEndpoints
{
	public static WebApplication MapProductEndpoints(
		this WebApplication app)
	{
		var routeGroupBuilder = app.MapGroup("/api/products");

		routeGroupBuilder.MapGet("/", GetProduct)
			.WithName("GetProduct");

		return app;
	}

	private static IResult GetProduct()
	{

		return Results.Ok();
	}
}