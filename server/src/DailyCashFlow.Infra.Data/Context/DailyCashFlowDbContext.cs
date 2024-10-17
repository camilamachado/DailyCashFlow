using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Domain.Features.DailyBalances;
using DailyCashFlow.Domain.Features.Transactions;
using DailyCashFlow.Domain.Features.Users;
using DailyCashFlow.Infra.Data.Features.Categories;
using DailyCashFlow.Infra.Data.Features.DailyBalances;
using DailyCashFlow.Infra.Data.Features.Transactions;
using DailyCashFlow.Infra.Data.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace DailyCashFlow.Infra.Data.Context
{
	public class DailyCashFlowDbContext : DbContext
	{
		public DailyCashFlowDbContext(DbContextOptions<DailyCashFlowDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<DailyBalance> DailyBalances { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
			modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());
			modelBuilder.ApplyConfiguration(new TransactionEntityConfiguration());
			modelBuilder.ApplyConfiguration(new DailyBalanceEntityConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}
