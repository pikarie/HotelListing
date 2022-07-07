using HotelListing.Core.Models.Hotel;

namespace HotelListing.Core.Models.Country
{
	public class CountryDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Alpha3Code { get; set; }
		public List<HotelDto>? Hotels { get; set; }
	}
}
