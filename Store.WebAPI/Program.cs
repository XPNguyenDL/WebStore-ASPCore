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
	app.SetupRequestPipeline();

	// Config endpoint;

	app.Run();
}

