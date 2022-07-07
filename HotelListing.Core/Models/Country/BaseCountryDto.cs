using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core.Models.Country
{
	public abstract class BaseCountryDto
	{
		[Required]
		public string Name { get; set; }

		[MaxLength(3)]
		public string Alpha3Code { get; set; }
	}
}
