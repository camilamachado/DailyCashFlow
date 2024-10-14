using DailyCashFlow.Infra.ResultPattern;

namespace DailyCashFlow.Domain.Features.Transactions
{
	public interface ITransactionRepository
	{
		/// <summary>
		/// Adiciona uma nova transação ao repositório.
		/// </summary>
		/// <param name="transaction">A transação a ser adicionada.</param>
		/// <returns>Retorna um resultado contendo a transação adicionada em caso de sucesso ou uma exceção em caso de falha.</returns>
		Task<Result<Transaction, Exception>> AddAsync(Transaction transaction);

		/// <summary>
		/// Verifica se já existe uma transação com os mesmos valores de categoria, data, tipo, valor e descrição.
		/// </summary>
		/// <param name="transaction">A transação para a qual verificar duplicidade.</param>
		/// <returns>Verdadeiro se uma transação duplicada for encontrada, false caso contrário.</returns>
		Task<Result<bool, Exception>> HasDuplicateTransactionAsync(Transaction transaction);
	}
}
