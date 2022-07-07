﻿using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core.Models.Hotel
{
	public abstract class BaseHotelDto
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Address { get; set; }

		public decimal? Rating { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int CountryId { get; set; }
	}
}
