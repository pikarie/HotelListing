﻿using HotelListing.API.Contracts;
using HotelListing.API.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthManager _authManager;

		public AuthenticationController(IAuthManager authManager)
		{
			this._authManager = authManager;
		}

		// POST: api/Authentication/register-admin
		[HttpPost]
		[Route("registerAdmin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> RegisterAdmin(ApiUserDto apiUserDto)
		{
			var errors = await _authManager.RegisterWithAdminRole(apiUserDto);

			if (errors.Any())
			{
				foreach (var error in errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
				}
				return BadRequest(ModelState);
			}

			return Ok();
		}

		// POST: api/Authentication/register
		[HttpPost]
		[Route("register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> Register(ApiUserDto apiUserDto)
		{
			var errors = await _authManager.RegisterWithUserRole(apiUserDto);

			if (errors.Any())
			{
				foreach (var error in errors)
				{
					ModelState.AddModelError(error.Code, error.Description);
				}
				return BadRequest(ModelState);
			}

			return Ok();
		}

		// POST: api/Authentication/login
		[HttpPost]
		[Route("login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
		{
			var authResponseDto = await _authManager.Login(loginDto);

			if (authResponseDto == null)
			{ 
				return Unauthorized();
			}

			return Ok(authResponseDto);
		}

		// POST: api/Authentication/refreshToken
		[HttpPost]
		[Route("refreshToken")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<AuthResponseDto>> RefreshToken(AuthResponseDto authResponseDto)
		{
			var authResponseDtoResult = await _authManager.VerifyRefreshToken(authResponseDto);

			if (authResponseDtoResult == null)
			{
				return Unauthorized();
			}

			return Ok(authResponseDtoResult);
		}
	}
}
