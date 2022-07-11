using AutoMapper;
using AutoMapper.QueryableExtensions;
using HotelListing.Core.Contracts;
using HotelListing.Core.Exceptions;
using HotelListing.Core.Models.Country;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Core.Repositories
{
	public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
	{
		private readonly HotelListingDbContext _context;
		private readonly IMapper _mapper;

		public CountriesRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<CountryDto> GetDetails(int id)
		{
			var country = await _context.Countries
							.Include(x => x.Hotels)
							.ProjectTo<CountryDto>(_mapper.ConfigurationProvider)
							.FirstOrDefaultAsync(x => x.Id == id);

			if (country == null)
			{
				throw new NotFoundException(nameof(GetDetails), id);
			}

			return country;
		}
	}
}
