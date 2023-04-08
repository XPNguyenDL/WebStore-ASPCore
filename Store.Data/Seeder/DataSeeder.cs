using Store.Core.Entities;
using Store.Core.Identity;
using Store.Data.Contexts;
using System.Collections.Generic;

namespace Store.Data.Seeder;

public class DataSeeder : IDataSeeder
{
	private readonly StoreDbContext _dbContext;
	private readonly IPasswordHasher _hasher;

	public DataSeeder(StoreDbContext dbContext, IPasswordHasher hasher)
	{
		_dbContext = dbContext;
		_hasher = hasher;
	}

	public void Initialize()
	{
		_dbContext.Database.EnsureCreated();

		if (_dbContext.Products.Any())
		{
			return;
		}

		var categories = AddCategories();
		var discounts = AddDiscount();
		var products = AddProduct(categories);
		var orders = AddOrder(products, discounts); 

		var roles = AddRoles();
		var users = AddUsers(roles);
	}

	private IList<Role> AddRoles()
	{
		var roles = new List<Role>()
		{
			new() {Id = Guid.NewGuid(), Name = "Admin"},
			new() {Id = Guid.NewGuid(), Name = "Manager"},
		};

		_dbContext.Roles.AddRange(roles);
		_dbContext.SaveChanges();
		return roles;
	}

	private IList<User> AddUsers(IList<Role> roles)
	{
		var users = new List<User>()
		{
			new User()
			{
				Id = Guid.NewGuid(),
				Name = "Admin",
				Email = "Admin@gmail.com",
				Address = "DLU",
				Phone = "0123456789",
				Username = "admin",
				Password = _hasher.Hash("admin123"),
				Roles = new List<Role>()
				{
					roles[0],
					roles[1]
				}
			}
		};

		_dbContext.Users.AddRange(users);
		_dbContext.SaveChanges();

		return users;
	}

	private IList<Category> AddCategories()
	{
		var categories = new List<Category>()
		{
			new() {Id = Guid.NewGuid(), Name = "Light novel", Description = "Light novel", UrlSlug = "light-novel"},
			new() {Id = Guid.NewGuid(), Name = "Manga", Description = "Manga", UrlSlug = "manga"},
			new() {Id = Guid.NewGuid(), Name = "Novel", Description = "Novel", UrlSlug = "novel"},
			new() {Id = Guid.NewGuid(), Name = "Comic", Description = "Comic", UrlSlug = "comic"},
		};

		_dbContext.Categories.AddRange(categories);
		_dbContext.SaveChanges();
		return categories;
	}

	private IList<Discount> AddDiscount()
	{
		var discount = new List<Discount>()
		{
			new() {Id = Guid.NewGuid(), Code = "100000001", Active = true, DiscountPercentage = 50, ExpiryDate = DateTime.Now.AddMonths(2), MinPrice = 0, Quantity = 100},
			new() {Id = Guid.NewGuid(), Code = "100000002", Active = true, DiscountPercentage = 50, ExpiryDate = DateTime.Now.AddMonths(2), MinPrice = 0, Quantity = 100},
			new() {Id = Guid.NewGuid(), Code = "100000003", Active = true, DiscountPercentage = 50, ExpiryDate = DateTime.Now.AddMonths(2), MinPrice = 0, Quantity = 100},
			new() {Id = Guid.NewGuid(), Code = "100000004", Active = true, DiscountPercentage = 50, ExpiryDate = DateTime.Now.AddMonths(2), MinPrice = 0, Quantity = 100},
		};

		_dbContext.Discounts.AddRange(discount);
		_dbContext.SaveChanges();
		return discount;
	}

	private IList<Order> AddOrder(IList<Product> products, IList<Discount> discounts)
	{
		var orders = new List<Order>()
		{
			new ()
			{
				Id = Guid.NewGuid(),
				Email = "2014478@gmail.com",
				FirstName = "Phát",
				OrderDate = DateTime.Now,
				ShipAddress = "DLU",
				ShipTel = "012345678",
				Status = OrderStatus.New,
				
			},
			new ()
			{
				Id = Guid.NewGuid(),
				Email = "2014478@gmail.com",
				FirstName = "Phát",
				OrderDate = DateTime.Now,
				ShipAddress = "DLU",
				ShipTel = "012345678",
				Status = OrderStatus.New,
				Discount = discounts[0],
				Details = new List<OrderDetail>()
				{
					new ()
					{
						ProductId = products[0].Id
					},
					new ()
					{
						ProductId = products[1].Id
					}
				}
			}
		};

		_dbContext.Orders.AddRange(orders);
		_dbContext.SaveChanges();
		return orders;
	}

	private IList<Product> AddProduct(IList<Category> categories)
	{
		var product = new List<Product>()
		{
			new ()
			{
				Id = Guid.NewGuid(),
				Quantity = 10,
				Name = "Mừng cậu trở về",
				CreateDate = DateTime.Now,
				CategoryId = categories[0].Id,
				ShortIntro = "Một Kaze hòa đồng, tươi sáng và một Moto giản đơn, nghiêm túc lại là bạn thân từ nhỏ.",
				Description = "Dù khi trưởng thành và có những nhóm bạn khác nhau, hai chàng trai vẫn luôn hiểu rõ người kia hơn ai hết." +
				              " Cho đến một ngày, Kaze chợt nhận ra tình cảm mình dành cho Moto đã vượt quá ngưỡng tình bạn." +
				              " Dẫu có cố gắng coi tình cảm ấy là hiểu nhầm đến thế nào hay cố gắng gạt bỏ nó ra sao, " +
				              "Kazu cũng dần phải thừa nhận, tấm chân tình này là thật…",
				Active = true,
				Discount = 10,
				Price = 100000,
				UrlSlug = "mung-cau-tro-ve",
			},
			new ()
			{
				Id = Guid.NewGuid(),
				Quantity = 10,
				CreateDate = DateTime.Now,
				Name = "Mừng cậu trở về",
				CategoryId = categories[0].Id,
				ShortIntro = "Một Kaze hòa đồng, tươi sáng và một Moto giản đơn, nghiêm túc lại là bạn thân từ nhỏ.",
				Description = "Dù khi trưởng thành và có những nhóm bạn khác nhau, hai chàng trai vẫn luôn hiểu rõ người kia hơn ai hết." +
				              " Cho đến một ngày, Kaze chợt nhận ra tình cảm mình dành cho Moto đã vượt quá ngưỡng tình bạn." +
				              " Dẫu có cố gắng coi tình cảm ấy là hiểu nhầm đến thế nào hay cố gắng gạt bỏ nó ra sao, " +
				              "Kazu cũng dần phải thừa nhận, tấm chân tình này là thật…",
				Active = true,
				Discount = 10,
				Price = 100000,
				UrlSlug = "mung-cau-tro-ve",
			}
		};

		_dbContext.Products.AddRange(product);
		_dbContext.SaveChanges();
		return product;
	}


}