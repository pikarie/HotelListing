using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
	public class HotelListingDbContext : DbContext
	{
		public HotelListingDbContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<Hotel> Hotels { get; set; }
		public DbSet<Country> Countries { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Country>().HasData(
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

			modelBuilder.Entity<Hotel>().HasData(
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
