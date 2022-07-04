using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Data
{
	public class Hotel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		[Precision(18, 2)]
		public decimal Rating { get; set; }

		[ForeignKey(nameof(CountryId))]
		public int CountryId { get; set; }
		public Country Country { get; set; }
	}
}
