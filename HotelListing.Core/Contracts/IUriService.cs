using HotelListing.Core.Models.Filter;

namespace HotelListing.Core.Contracts
{
	public interface IUriService
	{
		public Uri GetPageUri(PaginationFilter filter, string route);
	}
}
