﻿using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Domain.Features.Users;
using DailyCashFlow.Infra.Data.Features.Categories;
using DailyCashFlow.Infra.Data.Features.Users;
using Microsoft.EntityFrameworkCore;

namespace DailyCashFlow.Infra.Data.Context
{
	public class DailyCashFlowDbContext : DbContext
	{
		public DailyCashFlowDbContext(DbContextOptions<DailyCashFlowDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new UserEntityConfiguration());
			modelBuilder.ApplyConfiguration(new CategoryEntityConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}
