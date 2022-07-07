﻿using System.ComponentModel.DataAnnotations;

namespace HotelListing.Core.Models.User
{
	public class ApiUserDto
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(8)]
		public string Password { get; set; }
	}
}