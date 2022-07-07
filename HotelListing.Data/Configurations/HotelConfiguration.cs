using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Data.Configurations
{
	public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
	{
		public void Configure(EntityTypeBuilder<Hotel> builder)
		{
			builder.HasData(
				new List<Hotel>()
				{
					new()
					{
						Id = 1,
						Name = "Relax Moose Resort",
						Address = "108 Chestnut St, Toronto, ON, M5G1R3",
						Rating = 4.5M,
						CountryId = 1
					},
					new()
					{
						Id = 2,
						Name = "Beaver Spa",
						Address = "6546 Fallsview Blvd, Niagara Falls, ON, L2G2W2",
						Rating = 3.9M,
						CountryId = 1
					},
					new()
					{
						Id = 3,
						Name = "Yes Hotel",
						Address = "143 York St, Sydney NSW 2000, Australia",
						Rating = 4.8M,
						CountryId = 6
					}
				}
			);
		}
	}
}
