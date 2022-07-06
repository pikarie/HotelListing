using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.Configurations
{
	public class CountryConfiguration : IEntityTypeConfiguration<Country>
	{
		public void Configure(EntityTypeBuilder<Country> builder)
		{
			builder.HasData(
				new List<Country>()
				{
					new()
					{
						Id = 1,
						Name = "Canada",
						Alpha3Code = "CAN"
					},
					new()
					{
						Id = 2,
						Name = "Denmark",
						Alpha3Code = "DNK"
					},
					new()
					{
						Id = 3,
						Name = "Sweden",
						Alpha3Code = "SWE"
					},
					new()
					{
						Id = 4,
						Name = "Norway",
						Alpha3Code = "NOR"
					},
					new()
					{
						Id = 5,
						Name = "Switzerland",
						Alpha3Code = "CHE"
					},
					new()
					{
						Id = 6,
						Name = "Australia",
						Alpha3Code = "AUS"
					}
				}
			);
		}
	}
}
