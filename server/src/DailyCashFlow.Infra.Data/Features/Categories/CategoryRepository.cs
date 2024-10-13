using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Infra.Data.Context;
using DailyCashFlow.Infra.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace DailyCashFlow.Infra.Data.Features.Categories
{
	public class CategoryRepository : ICategoryRepository
	{
		private readonly DailyCashFlowDbContext _context;

		public CategoryRepository(DailyCashFlowDbContext context)
		{
			_context = context;
		}

		public async Task<Result<Category, Exception>> AddAsync(Category category)
		{
			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return category;
		}

		public Result<IQueryable<Category>, Exception> GetAll()
		{
			try
			{
				var query = _context.Categories.AsQueryable();

				return Result<IQueryable<Category>, Exception>.Ok(query);
			}
			catch (Exception ex)
			{
				return ex;
			}
		}

		public async Task<Result<bool, Exception>> HasAnyByNameAsync(string name)
		{
			var hasAny = await _context.Categories.AnyAsync(u => u.Name.ToLower() == name.ToLower());

			return hasAny;
		}
	}
}
