﻿using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.User
{
	public class LoginDto
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[MinLength(8)]
		public string Password { get; set; }
	}
}