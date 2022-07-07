using AutoMapper;
using HotelListing.API.Contracts;
using HotelListing.API.Data;
using HotelListing.API.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListing.API.Repositories
{
	public class AuthService : IAuthService
	{
		private readonly IMapper _mapper;
		private readonly UserManager<ApiUser> _userManager;
		private readonly IConfiguration _configuration;
		private readonly ILogger<AuthService> _logger;
		private readonly string roleAdmin = "Administrator";
		private readonly string roleUser = "User";
		private readonly string loginProvider = "HotelListingApi";
		private readonly string tokenName = "RefreshToken";

		private ApiUser user;

		public AuthService(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration, ILogger<AuthService> logger)
		{
			_mapper = mapper;
			_userManager = userManager;
			_configuration = configuration;
			_logger = logger;
		}

		public async Task<IEnumerable<IdentityError>> RegisterWithAdminRole(ApiUserDto userDto)
		{
			var user = _mapper.Map<ApiUser>(userDto);
			user.UserName = userDto.Email;

			var result = await _userManager.CreateAsync(user, userDto.Password);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, roleAdmin);
			}

			return result.Errors;
		}

		public async Task<IEnumerable<IdentityError>> RegisterWithUserRole(ApiUserDto userDto)
		{
			user = _mapper.Map<ApiUser>(userDto);
			user.UserName = userDto.Email;

			var result = await _userManager.CreateAsync(user, userDto.Password);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, roleUser);
			}

			return result.Errors;
		}

		public async Task<AuthResponseDto> Login(LoginDto loginDto)
		{
			user = await _userManager.FindByEmailAsync(loginDto.Email);
			bool isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

			if (user == null || !isValid)
			{
				_logger.LogInformation("User with email {Email} was not found.", loginDto.Email);
				return null;
			}

			var token = await GenerateToken();
			return new AuthResponseDto
			{
				Token = token,
				UserId = user.Id,
				RefreshToken = await CreateRefreshToken()
			};
		}

		private async Task<string> GenerateToken()
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var roles = await _userManager.GetRolesAsync(user);
			var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
			var userClaims = await _userManager.GetClaimsAsync(user);

			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim("uid", user.Id),
			}
			.Union(userClaims)
			.Union(roleClaims);

			var token = new JwtSecurityToken(
				issuer: _configuration["JwtSettings:Issuer"],
				audience: _configuration["JwtSettings:Audience"],
				claims: claims,
				expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
				signingCredentials: credentials
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<string> CreateRefreshToken()
		{
			await _userManager.RemoveAuthenticationTokenAsync(user, loginProvider, tokenName);
			var newRefreshToken = await _userManager.GenerateUserTokenAsync(user, loginProvider, tokenName);
			var result = await _userManager.SetAuthenticationTokenAsync(user, loginProvider, tokenName, newRefreshToken);

			return newRefreshToken;
		}

		public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto authResponseDto)
		{
			var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(authResponseDto.Token);
			var username = tokenContent.Claims.ToList().FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value;

			user = await _userManager.FindByNameAsync(username);

			if (user == null || user.Id != authResponseDto.UserId)
			{
				return null;
			}

			var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(user,loginProvider,tokenName,authResponseDto.RefreshToken);

			if (isValidRefreshToken)
			{
				var token = await GenerateToken();
				return new AuthResponseDto
				{
					UserId = user.Id,
					Token = token,
					RefreshToken = await CreateRefreshToken()
				};
			}

			await _userManager.UpdateSecurityStampAsync(user);
			return null;
		}
	}
}
