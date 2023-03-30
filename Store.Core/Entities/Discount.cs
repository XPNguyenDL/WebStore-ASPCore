using Store.Core.Contracts;

namespace Store.Core.Entities;

public class Discount : IEntity
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Code { get; set; }

	public float DiscountPercentage { get; set; }

	public bool Active { get; set; }
}