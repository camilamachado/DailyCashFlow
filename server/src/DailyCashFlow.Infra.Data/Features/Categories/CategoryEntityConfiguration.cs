using DailyCashFlow.Domain.Features.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyCashFlow.Infra.Data.Features.Categories
{
	public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
	{
		public void Configure(EntityTypeBuilder<Category> builder)
		{
			builder.ToTable("Categories");

			builder.HasKey(sc => sc.Id);

			builder.Property(y => y.Name).IsRequired().HasMaxLength(50);
		}
	}
}
