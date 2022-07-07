using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Repositories
{
	public class HotelRepository : GenericRepository<Hotel>, IHotelsRepository
	{
		private readonly HotelListingDbContext _context;

		public HotelRepository(HotelListingDbContext context, IMapper mapper) : base(context, mapper)
		{
			_context = context;
		}

		public async Task<Hotel> GetDetails(int id)
		{
			Hotel? hotel = await _context.Hotels
							.Include(x => x.Country)
							.FirstOrDefaultAsync(x => x.Id == id);
			return hotel;
		}
	}
}
