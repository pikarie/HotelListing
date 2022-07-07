using AutoMapper;
using HotelListing.Core.Contracts;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Core.Repositories
{
	public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
	{
		private readonly HotelListingDbContext _context;

		public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
		{
			_context = context;
		}

		public async Task<Country> GetDetails(int id)
		{
			Country? country = await _context.Countries
							.Include(x => x.Hotels)
							.FirstOrDefaultAsync(x => x.Id == id);
			return country;
		}
	}
}
