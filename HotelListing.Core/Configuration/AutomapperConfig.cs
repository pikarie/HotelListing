using AutoMapper;
using HotelListing.Core.Models.Country;
using HotelListing.Core.Models.Hotel;
using HotelListing.Core.Models.User;
using HotelListing.Data;

namespace HotelListing.Core.Configuration
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
