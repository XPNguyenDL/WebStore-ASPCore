﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities;

namespace Store.Data.Mappings;

public class FeedbackMap : IEntityTypeConfiguration<Feedback>
{
	public void Configure(EntityTypeBuilder<Feedback> builder)
	{
		builder.HasKey(p => p.Id);

		builder.ToTable("Feedbacks");

		builder.Property(p => p.UserID)
			.IsRequired()
			.HasMaxLength(512);

		builder.Property(s => s.Rate)
			.IsRequired()
			.HasDefaultValue(0);

		builder.Property(p => p.Content)
			.HasMaxLength(500);

		builder.Property(s => s.PostDate)
			.IsRequired()
			.HasColumnType("datetime")
			.HasAnnotation("Range", new[] { 0, 5 });

		builder.HasOne(s => s.Product)
			.WithMany(s => s.Feedbacks)
			.HasForeignKey(s => s.Id)
			.OnDelete(DeleteBehavior.Cascade);
	}
}