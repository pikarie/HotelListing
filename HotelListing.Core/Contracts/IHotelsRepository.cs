using HotelListing.Data;

namespace HotelListing.Core.Contracts
{
	public interface IHotelsRepository : IGenericRepository<Hotel>
	{
		Task<Hotel> GetDetails(int id);
	}
}
