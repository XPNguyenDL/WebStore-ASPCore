using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Core.Entities;

namespace Store.Data.Mappings;

public class PictureMap : IEntityTypeConfiguration<Picture>
{
	public void Configure(EntityTypeBuilder<Picture> builder)
	{
		builder.HasKey(p => p.Id);

		builder.ToTable("Picture");

		builder.Property(p => p.Path)
			.IsRequired()
			.HasMaxLength(512);

		builder.Property(p => p.Active)
			.IsRequired()
			.HasDefaultValue(false);

		builder.HasOne(s => s.Product)
			.WithMany(s => s.Pictures)
			.HasForeignKey(s => s.ProductId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}