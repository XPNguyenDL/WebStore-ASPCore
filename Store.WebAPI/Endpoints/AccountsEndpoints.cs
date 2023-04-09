using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Store.Core.Entities;
using Store.Services.Shops;
using Store.WebAPI.Identities;
using Store.WebAPI.Models;
using Store.WebAPI.Models.UserModel;

namespace Store.WebAPI.Endpoints;

public static class AccountsEndpoints
{
	public static WebApplication MapAccountEndpoints(
		this WebApplication app)
	{
		var routeGroupBuilder = app.MapGroup("/api/accounts");

		routeGroupBuilder.MapPost("/", Login)
			.WithName("Login")
			.AllowAnonymous();

		routeGroupBuilder.MapPost("/Register", Register)
			.WithName("Register");

		return app;
	}

	private static async Task<IResult> Login(
		[FromBody] UserLogin userLogin,
		[FromServices] IUserRepository repository,
		[FromServices] IConfiguration configuration,
		[FromServices] IMapper mapper)
	{

		var user = IdentityManager.Authenticate(userLogin, repository, mapper);

		UserDto userDto = await user;
		if (userDto != null)
		{
			var token = IdentityManager.Generate(userDto, configuration);

			var accessToken = new AccessTokenModel()
			{
				Token = new JwtSecurityTokenHandler().WriteToken(token),
				UserDto = userDto,
				ExpiresToken = token.ValidTo,
				TokenType = "bearer"
			};

			return Results.Ok(accessToken);
		}

		return Results.NotFound("User not found");
	}

	private static async Task<IResult> Register(
		[FromBody] UserEditModel model,
		[FromServices] IUserRepository repository,
		[FromServices] IConfiguration configuration,
		[FromServices] IMapper mapper)
	{
		var user = mapper.Map<User>(model);

		var userExist = await repository.IsUserExistedAsync(user.Username);
		if (userExist)
		{
			return Results.NotFound("Account is exist");
		}

		var newUser = await repository.Register(user, model.ListRoles);

		var userDto = mapper.Map<UserDto>(newUser);

		return Results.Ok(userDto);
	}


}