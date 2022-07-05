using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Models.Country
{
	public class CountryDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Alpha3Code { get; set; }
		public List<HotelDto>? Hotels { get; set; }
	}
}
