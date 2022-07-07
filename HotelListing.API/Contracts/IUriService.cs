using HotelListing.API.Models.Filter;

namespace HotelListing.API.Contracts
{
	public interface IUriService
	{
		public Uri GetPageUri(PaginationFilter filter, string route);
	}
}
