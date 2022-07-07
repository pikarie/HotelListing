using AutoMapper;
using HotelListing.Core.Contracts;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Core.Repositories
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
