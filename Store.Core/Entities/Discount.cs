﻿using Store.Core.Contracts;

namespace Store.Core.Entities;

public class Discount : IEntity
{
	public Guid Id { get; set; }

	public int Quantity { get; set; }

	public float MinPrice { get; set; }

	public DateTime ExpiryDate { get; set; }

	public string Code { get; set; }

	public float DiscountPercentage { get; set; }

	public bool Active { get; set; }
}