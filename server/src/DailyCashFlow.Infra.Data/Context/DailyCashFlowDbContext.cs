using DailyCashFlow.Domain.Features.Users;
using DailyCashFlow.Infra.Data.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace DailyCashFlow.Infra.Data.Context
{
	public class DailyCashFlowDbContext : DbContext
	{
		public DailyCashFlowDbContext(DbContextOptions<DailyCashFlowDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}
