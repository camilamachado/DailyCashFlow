using DailyCashFlow.Domain.Features.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyCashFlow.Infra.Data.Features.Transactions
{
	public class TransactionEntityConfiguration : IEntityTypeConfiguration<Transaction>
	{
		public void Configure(EntityTypeBuilder<Transaction> builder)
		{
			builder.ToTable("Transactions", t =>
			{
				t.HasCheckConstraint("CK_Transactions_Type", "Type IN ('C', 'D')");
			});

			builder.HasKey(t => t.Id);

			builder.HasIndex(t => t.Date).IsUnique(false);

			builder.Property(t => t.CategoryId)
				.IsRequired();

			builder.Property(t => t.Date)
				.IsRequired()
				.HasColumnType("datetime2");

			builder.Property(t => t.Type)
				.HasConversion(
					v => v == TransactionType.Credit ? 'C' : 'D', 
					v => v == 'C' ? TransactionType.Credit : TransactionType.Debit
				)
				.HasColumnType("char(1)")
				.IsRequired();

			builder.Property(t => t.Amount)
				.IsRequired()
				.HasColumnType("decimal(18,2)");

			builder.Property(t => t.Description)
				.HasMaxLength(255);

			builder.Property(t => t.IsCancelled)
				.IsRequired();

			builder.HasOne(t => t.Category)
				.WithMany(c => c.Transactions)
				.HasForeignKey(t => t.CategoryId)
				.OnDelete(DeleteBehavior.Restrict); 
		}
	}
}