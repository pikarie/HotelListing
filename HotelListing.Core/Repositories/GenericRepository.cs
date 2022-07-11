using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Core.Contracts;
using HotelListing.Core.Exceptions;
using HotelListing.Core.Models.Filter;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Core.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly HotelListingDbContext _context;
		private readonly IMapper _mapper;

		public GenericRepository(HotelListingDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<List<T>> GetAllAsync()
		{
			return await _context.Set<T>().ToListAsync();
		}

		public async Task<List<TResult>> GetAllAsync<TResult>(PaginationFilter filter)
		{
			var totalRecords = await _context.Set<T>().CountAsync();
			var items = await _context.Set<T>()
				.Skip((filter.PageNumber - 1) * filter.PageSize)
				.Take(filter.PageSize)
				.ProjectTo<TResult>(_mapper.ConfigurationProvider)
				.ToListAsync();

			return items;
		}

		public async Task<List<TResult>> GetAllAsync<TResult>()
		{
			return await _context.Set<T>()
				.ProjectTo<TResult>(_mapper.ConfigurationProvider)
				.ToListAsync();
		}

		public async Task<T> GetAsync(int? id)
		{
			if (id is null)
			{
				return null;
			}

			return await _context.Set<T>().FindAsync(id);
		}

		public async Task<TResult> GetAsync<TResult>(int? id)
		{
			var result = await _context.Set<T>().FindAsync(id);

			if (result is null)
			{
				throw new NotFoundException(typeof(T).Name, id.HasValue ? id : "invalid id");
			}

			return _mapper.Map<TResult>(result);
		}

		public async Task<T> AddAsync(T entity)
		{
			await _context.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		/// <summary>
		/// "T" would be the entity, e.g. Hotel
		/// </summary>
		/// <typeparam name="TSource">A dto, e.g. CreateHotelDto</typeparam>
		/// <typeparam name="TResult">A dto, e.g. HotelDto</typeparam>
		/// <param name="source"></param>
		/// <returns></returns>
		public async Task<TResult> AddAsync<TSource, TResult>(TSource source)
		{
			var entity = _mapper.Map<T>(source);

			await _context.AddAsync(entity);
			await _context.SaveChangesAsync();

			return _mapper.Map<TResult>(entity);
		}

		public async Task<T> UpdateAsync(T entity)
		{
			_context.Update(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task UpdateAsync<TSource>(int id, TSource source)
		{
			var entity = await GetAsync(id);

			if (entity == null)
			{
				throw new NotFoundException(typeof(T).Name, id);
			}

			_mapper.Map(source, entity);
			_context.Update(entity);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var entity = await GetAsync(id);

			if (entity is null)
			{
				throw new NotFoundException(typeof(T).Name, id);
			}

			_context.Set<T>().Remove(entity);
			await _context.SaveChangesAsync();
		}

		public async Task<bool> Exists(int id)
		{
			var entity = await GetAsync(id);
			return entity != null;
		}

		public async Task<int> GetCountAsync()
		{
			return await _context.Set<T>().CountAsync();
		}
	}
}
