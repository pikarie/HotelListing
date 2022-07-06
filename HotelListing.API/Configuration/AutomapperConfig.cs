using AutoMapper;
using HotelListing.API.Data;
using HotelListing.API.Models.Country;
using HotelListing.API.Models.Hotel;
using HotelListing.API.Models.User;

namespace HotelListing.API.Configuration
{
	public class AutomapperConfig : Profile
	{
		public AutomapperConfig()
		{
			MapCountry();
			MapHotel();
			MapApiUser();
		}

		private void MapCountry()
		{
			CreateMap<Country, CountryDto>().ReverseMap();
			CreateMap<Country, GetCountryDto>().ReverseMap();
			CreateMap<Country, CreateCountryDto>().ReverseMap();
			CreateMap<Country, ModifyCountryDto>().ReverseMap();
		}

		private void MapHotel()
		{
			CreateMap<Hotel, HotelDto>().ReverseMap();
			CreateMap<Hotel, CreateHotelDto>().ReverseMap();
		}

		private void MapApiUser()
		{
			CreateMap<ApiUser, ApiUserDto>().ReverseMap();
		}
	}
}
