using HotelListing.API.Contracts;
using HotelListing.API.Models.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticationController : ControllerBase
	{
		private readonly IAuthService _authManager;
		private readonly ILogger<AuthenticationController> _logger;

		public AuthenticationController(IAuthService authManager, ILogger<AuthenticationController> logger)
		{
			_authManager = authManager;
			_logger = logger;
		}

		// POST: api/Authentication/register-admin
		[HttpPost]
		[Route("registerAdmin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> RegisterAdmin(ApiUserDto apiUserDto)
		{
			try
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
			catch (Exception e)
			{
				_logger.LogError(e, "An exception occurred in method {NameOfMethod} for user with email {Email}.",
					nameof(RegisterAdmin), apiUserDto.Email);
				return Problem($"An exception occurred in method {nameof(RegisterAdmin)} for user with email {apiUserDto.Email}. Please contact support.",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		}

		// POST: api/Authentication/register
		[HttpPost]
		[Route("register")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> Register(ApiUserDto apiUserDto)
		{
			try
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
			catch (Exception e)
			{
				_logger.LogError(e, "An exception occurred in method {NameOfMethod} for user with email {Email}.",
					nameof(Register), apiUserDto.Email);
				return Problem($"An exception occurred in method {nameof(Register)} for user with email {apiUserDto.Email}. Please contact support.",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		}

		// POST: api/Authentication/login
		[HttpPost]
		[Route("login")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
		{
			try
			{
				var authResponseDto = await _authManager.Login(loginDto);

				if (authResponseDto == null)
				{
					return Unauthorized();
				}

				return Ok(authResponseDto);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "An exception occurred in method {NameOfMethod} for user with email {Email}.",
					nameof(Login), loginDto.Email);
				return Problem($"An exception occurred in method {nameof(Login)} for user with email {loginDto.Email}. Please contact support.",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		}

		// POST: api/Authentication/refreshToken
		[HttpPost]
		[Route("refreshToken")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public async Task<ActionResult<AuthResponseDto>> RefreshToken(AuthResponseDto authResponseDto)
		{
			try
			{
				var authResponseDtoResult = await _authManager.VerifyRefreshToken(authResponseDto);

				if (authResponseDtoResult == null)
				{
					return Unauthorized();
				}

				return Ok(authResponseDtoResult);
			}
			catch (Exception e)
			{
				_logger.LogError(e, "An exception occurred in method {NameOfMethod} for user id {UserId}.",
					nameof(RefreshToken), authResponseDto.UserId);
				return Problem($"An exception occurred in method {nameof(RefreshToken)} for user id {authResponseDto.UserId}. Please contact support.",
					statusCode: StatusCodes.Status500InternalServerError);
			}
		}
	}
}
