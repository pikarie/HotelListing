﻿using AutoMapper;
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
	public class AuthManager : IAuthManager
	{
		private readonly IMapper _mapper;
		private readonly UserManager<ApiUser> _userManager;
		private readonly IConfiguration _configuration;
		private readonly string roleUser = "User";

		public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
		{
			_mapper = mapper;
			_userManager = userManager;
			_configuration = configuration;
		}

		public async Task<IEnumerable<IdentityError>> Register(ApiUserDto userDto)
		{
			var user = _mapper.Map<ApiUser>(userDto);
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
			var user = await _userManager.FindByEmailAsync(loginDto.Email);
			bool isValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);

			if (user == null || !isValid)
			{
				return null;
			}

			var token = await GenerateToken(user);
			return new AuthResponseDto
			{
				UserId = user.Id,
				Token = token
			};
		}

		private async Task<string> GenerateToken(ApiUser user)
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
	}
}
