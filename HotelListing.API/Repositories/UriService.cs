using HotelListing.API.Contracts;
using HotelListing.API.Models.Filter;
using Microsoft.AspNetCore.WebUtilities;

namespace HotelListing.API.Repositories
{
	public class UriService : IUriService
	{
		private readonly string _baseUri;

		public UriService(string baseUri)
		{
			_baseUri = baseUri;
		}

		public Uri GetPageUri(PaginationFilter filter, string route)
		{
			var enpointUri = new Uri(string.Concat(_baseUri, route));
			var modifiedUri = QueryHelpers.AddQueryString(enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
			modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
			return new Uri(modifiedUri);
		}
	}
}
