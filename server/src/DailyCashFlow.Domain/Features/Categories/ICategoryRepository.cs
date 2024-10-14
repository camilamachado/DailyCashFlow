using DailyCashFlow.Infra.ResultPattern;

namespace DailyCashFlow.Domain.Features.Categories
{
	public interface ICategoryRepository
	{
		/// <summary>
		/// Adiciona uma nova categoria ao repositório.
		/// </summary>
		/// <param name="category">A categoria a ser adicionada.</param>
		/// <returns>Retorna um resultado contendo a categoria adicionada em caso de sucesso ou uma exceção em caso de falha.</returns>
		Task<Result<Category, Exception>> AddAsync(Category category);

		/// <summary>
		/// Verifica se já existe uma categoria com o nome fornecido.
		/// </summary>
		/// <param name="name">O nome da categoria a ser verificado.</param>
		/// <returns>Retorna um resultado contendo um valor booleano indicando se o nome da categoria existe ou não ou uma exceção em caso de falha..</returns>
		Task<Result<bool, Exception>> HasAnyByNameAsync(string name);

		/// <summary>
		/// Recupera todas as categorias do repositório.
		/// </summary>
		/// <returns>Retorna um resultado contendo uma lista de categorias em caso de sucesso ou uma exceção em caso de falha.</returns>
		Result<IQueryable<Category>, Exception> GetAll();

		/// <summary>
		/// Verifica se já existe uma categoria com o id fornecido.
		/// </summary>
		/// <param name="id">O id da categoria a ser verificado.</param>
		/// <returns>Retorna um resultado contendo um valor booleano indicando se o id da categoria existe ou não ou uma exceção em caso de falha..</returns>
		Task<Result<bool, Exception>> HasAnyByIdAsync(int id);
	}
}
