using DailyCashFlow.Domain.Features.DailyBalances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyCashFlow.Infra.Data.Features.DailyBalances
{
	public class DailyBalanceEntityConfiguration : IEntityTypeConfiguration<DailyBalance>
	{
		public void Configure(EntityTypeBuilder<DailyBalance> builder)
		{
			builder.ToTable("DailyBalances");

			builder.HasKey(db => db.Id);

			builder.HasIndex(db => db.Date).IsUnique();

			builder.Property(db => db.Date)
				.HasColumnType("date")
				.IsRequired();

			builder.Property(db => db.TotalCredit)
				.HasColumnType("decimal(18,2)")
				.IsRequired();

			builder.Property(db => db.TotalDebit)
				.HasColumnType("decimal(18,2)")
				.IsRequired();

			builder.Property(db => db.NetBalance)
				.HasColumnType("decimal(18,2)")
				.IsRequired();
		}
	}
}