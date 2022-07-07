using System.ComponentModel.DataAnnotations;

namespace HotelListing.Data
{
	public class Country
	{
		public int Id { get; set; }
		public string Name { get; set; }
		[MaxLength(3)]
		public string Alpha3Code { get; set; }

		public virtual ICollection<Hotel>? Hotels { get; set; }
	}
}
