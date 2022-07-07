using HotelListing.Core.Models.Filter;

namespace HotelListing.Core.Contracts
{
	public interface IGenericRepository<T> where T : class
	{
		Task<List<T>> GetAllAsync();
		Task<List<TResult>> GetAllAsync<TResult>(PaginationFilter filter);
		Task<T> GetAsync(int? id);
		Task<T> AddAsync(T entity);
		Task<T> UpdateAsync(T entity);
		Task DeleteAsync(int id);
		Task<bool> Exists(int id);
		Task<int> GetCountAsync();
	}
}
