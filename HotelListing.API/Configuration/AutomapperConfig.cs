using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotel;

namespace HotelListing.API.Configuration
{
	public class AutomapperConfig : Profile
	{
		public AutomapperConfig()
		{
			MapCountry();
			MapHotel();
		}

		private void MapCountry()
		{
			CreateMap<Country, CountryDto>().ReverseMap();
			CreateMap<Country, GetCountryDto>().ReverseMap();
			CreateMap<Country, PostCountryDto>().ReverseMap();
			CreateMap<Country, PutCountryDto>().ReverseMap();
		}

		private void MapHotel()
		{
			CreateMap<Hotel, HotelDto>().ReverseMap();
		}
	}
}
