using Store.WebAPI.Endpoints;
using Store.WebAPI.Extensions;
using Store.WebAPI.Mapsters;
using Store.WebAPI.Validations;

var builder = WebApplication.CreateBuilder(args);
{
	builder
		.ConfigureCors()
		.ConfigureNLog()
		.ConfigureServices()
		.ConfigureSwaggerOpenApi()
		.ConfigureMapster()
		.ConfigureFluentValidation();
}


var app = builder.Build();
{
	app.SetupContext();
	app.SetupRequestPipeline();
	
	// use seeder
	app.UseDataSeeder();

	// Config endpoint;
	app.MapProductEndpoints()
		.MapAccountEndpoints()
		.MapCategoriesEndpoint()
		.MapOrdersEndpoint()
		.MapDashboardEndpoint();

	app.Run();
}

