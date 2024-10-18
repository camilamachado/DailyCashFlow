using DailyCashFlow.Domain.Features.Categories;
using DailyCashFlow.Infra.ResultPattern;

namespace DailyCashFlow.Domain.Features.DailyBalances
{
	public interface IDailyBalanceRepository
	{
		/// <summary>
		/// Adiciona um registro de saldo diário ao repositório.
		/// </summary>
		/// <param name="dailyBalance">O saldo diário a ser adicionado.</param>
		/// <returns>Retorna um resultado contendo o saldo diário adicionado em caso de sucesso ou uma exceção em caso de falha.</returns>
		Task<Result<DailyBalance, Exception>> AddAsync(DailyBalance dailyBalance);

		#nullable enable
		/// <summary>
		/// Recupera um registro de saldo diário com base na data fornecida.
		/// </summary>
		/// <param name="date">A data a ser verificada.</param>
		/// <returns>Retorna um resultado contendo o saldo diário encontrado em caso de sucesso, 
		/// um resultado nulo se nenhum registro for encontrado, ou uma exceção em caso de falha.</returns>
		Task<Result<DailyBalance?, Exception>> GetByDateAsync(DateTime date);
		#nullable disable


		/// <summary>
		/// Atualiza um registro de saldo diário existente no repositório.
		/// </summary>
		/// <param name="dailyBalance">O saldo diário a ser atualizado.</param>
		/// <returns>Retorna um resultado contendo o saldo diário atualizado em caso de sucesso ou uma exceção em caso de falha.</returns>
		Task<Result<DailyBalance, Exception>> UpdateAsync(DailyBalance dailyBalance);

		/// <summary>
		/// Recupera todos registros de saldo diário do repositório.
		/// </summary>
		/// <returns>Retorna um resultado contendo uma lista de registros de saldo diário em caso de sucesso ou uma exceção em caso de falha.</returns>
		Result<IQueryable<DailyBalance>, Exception> GetAllNoTracking();
	}
}
