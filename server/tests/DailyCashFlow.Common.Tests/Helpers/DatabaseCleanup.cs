using DailyCashFlow.Infra.Data.Context;

namespace DailyCashFlow.Common.Tests.Helpers
{
	public static class DatabaseCleanup
	{
		public static void Cleanup(DailyCashFlowDbContext context)
		{
			// Remove todos os usuários, exceto o administrador padrão
			var usersToRemove = context.Users.Where(user => user.Email != "admin@admin.com");
			context.Users.RemoveRange(usersToRemove);


			context.SaveChanges();
		}
	}
}
