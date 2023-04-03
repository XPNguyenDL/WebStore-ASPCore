using Microsoft.EntityFrameworkCore;
using NLog.Web;
using Store.Data.Contexts;
using Store.Data.Seeder;
using Store.Services.Shops;
using Store.WebAPI.Media;

namespace Store.WebAPI.Extensions;

public static class WebApplicationExtensions
{
	public static WebApplicationBuilder ConfigureServices(
		this WebApplicationBuilder builder)
	{
		builder.Services.AddMemoryCache();

		builder.Services.AddDbContext<StoreDbContext>(
			option =>
				option.UseSqlServer(
					builder.Configuration.GetConnectionString("DefaultConnection")));

		builder.Services.AddScoped<IMediaManager, LocalFileSystemMediaManager>();
		builder.Services.AddScoped<IDataSeeder, DataSeeder>();
		builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();


		return builder;
	}

	public static WebApplicationBuilder ConfigureCors(
		this WebApplicationBuilder builder)
	{
		builder.Services.AddCors(option =>
			option.AddPolicy("StoreApp", policyBuilder =>
				policyBuilder
					.AllowAnyOrigin()
					.AllowAnyHeader()
					.AllowAnyMethod()));

		return builder;
	}

	public static WebApplicationBuilder ConfigureNLog(
		this WebApplicationBuilder builder)
	{
		builder.Logging.ClearProviders();
		builder.Host.UseNLog();

		return builder;
	}

	public static IApplicationBuilder UseDataSeeder(
		this IApplicationBuilder app)
	{
		using var scope = app.ApplicationServices.CreateScope();

		try
		{
			scope.ServiceProvider.GetRequiredService<IDataSeeder>().Initialize();
		}
		catch (Exception e)
		{
			scope.ServiceProvider.GetRequiredService<ILogger<Program>>()
				.LogError(e, "Count not insert data into database");
		}

		return app;
	}

	public static WebApplicationBuilder ConfigureSwaggerOpenApi(
		this WebApplicationBuilder builder)
	{
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		return builder;
	}

	public static WebApplication SetupRequestPipeline(
		this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseStaticFiles();

		app.UseHttpsRedirection();

		app.UseCors("StoreApp");

		return app;
	}

}