namespace Store.Core.Entities;

public class OrderDetail
{
	public Guid OrderId { get; set; }
	
	public Guid ProductId { get; set; }

	public int Quantity { get; set; }

	public double Price { get; set; }

	public double TotalPrice
	{
		get
		{
			return Math.Round(Price * Quantity * (1 - Order.Discount.DiscountPercentage), 2);
		}
	}

	// ======================================================
	// Navigation properties
	// ======================================================

	public virtual Order Order { get; set; }
	public virtual Product Product { get; set; }
}