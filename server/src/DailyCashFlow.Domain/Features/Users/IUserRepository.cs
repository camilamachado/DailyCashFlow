using DailyCashFlow.Infra.ResultPattern;

namespace DailyCashFlow.Domain.Features.Users
{
	public interface IUserRepository
	{
		/// <summary>
		/// Adiciona um novo usuário ao repositório.
		/// </summary>
		/// <param name="user">O usuário a ser adicionado.</param>
		/// <returns>Retorna um resultado contendo o usuário adicionado em caso de sucesso ou uma exceção em caso de falha.</returns>
		Task<Result<User, Exception>> AddAsync(User user);

		/// <summary>
		/// Verifica se já existe um usuário com o endereço de e-mail fornecido.
		/// </summary>
		/// <param name="email">O endereço de e-mail a ser verificado.</param>
		/// <returns>Retorna um resultado contendo um valor booleano indicando se o usuário existe ou não ou uma exceção em caso de falha..</returns>
		Task<Result<bool, Exception>> HasAnyByEmailAsync(string email);

		/// <summary>
		/// Recupera um usuário com base nas credenciais de e-mail e senha fornecidas.
		/// </summary>
		/// <param name="email">O endereço de e-mail do usuário.</param>
		/// <returns>Retorna um resultado contendo o usuário encontrado em caso de sucesso ou uma exceção em caso de falha.</returns>
		Task<Result<User, Exception>> GetByEmailNoTrackingAsync(string email);
	}

}
