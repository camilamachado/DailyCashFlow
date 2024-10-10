using DailyCashFlow.Domain.Exceptions;
using DailyCashFlow.Domain.Features.Users;
using DailyCashFlow.Infra.Data.Context;
using DailyCashFlow.Infra.ResultPattern;
using Microsoft.EntityFrameworkCore;

namespace DailyCashFlow.Infra.Data.Features.Users
{
	public class UserRepository : IUserRepository
	{
		private readonly DailyCashFlowDbContext _context;

		public UserRepository(DailyCashFlowDbContext context)
		{
			_context = context;
		}

		public async Task<Result<User, Exception>> AddAsync(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return user;
		}

		public async Task<Result<bool, Exception>> HasAnyByEmailAsync(string email)
		{
			var hasAny = await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());

			return hasAny;
		}

		public async Task<Result<User, Exception>> GetByEmailNoTrackingAsync(string email)
		{
			var user = await _context.Users
				.AsNoTracking()
				.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());

			if (user == null)
			{
				return new NotFoundException("User not found.");
			}

			return user;
		}

	}

}
