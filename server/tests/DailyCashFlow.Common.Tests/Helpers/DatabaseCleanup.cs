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

			// Remove todas as categorias, exceto as categorias padrão
			var defaultCategories = new[]
			{
				"Despesas Operacionais",
				"Compras",
				"Pagamentos",
				"Perdas",
				"Vendas",
				"Recebimentos",
				"Outras Receitas"
			};

			var categoriesToRemove = context.Categories.Where(category => !defaultCategories.Contains(category.Name));
			context.Categories.RemoveRange(categoriesToRemove);

			// Remove todas as transações
			var transactionsToRemove = context.Transactions.ToList();
			context.Transactions.RemoveRange(transactionsToRemove);

			context.SaveChanges();
		}
	}
}
