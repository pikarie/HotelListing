using HotelListing.Core.Models.Filter;

namespace HotelListing.Core.Contracts
{
	public interface IGenericRepository<T> where T : class
	{
		Task<List<T>> GetAllAsync();
		Task<List<TResult>> GetAllAsync<TResult>(PaginationFilter filter);
		Task<List<TResult>> GetAllAsync<TResult>();
		Task<T> GetAsync(int? id);
		Task<TResult> GetAsync<TResult>(int? id);
		Task<T> AddAsync(T entity);
		Task<TResult> AddAsync<TSource, TResult>(TSource source);
		Task<T> UpdateAsync(T entity);
		Task UpdateAsync<TSource>(int id, TSource source);
		Task DeleteAsync(int id);
		Task<bool> Exists(int id);
		Task<int> GetCountAsync();
	}
}
