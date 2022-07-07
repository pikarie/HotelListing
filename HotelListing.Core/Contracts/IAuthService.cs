using HotelListing.Core.Models.User;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.Core.Contracts
{
	public interface IAuthService
	{
		Task<IEnumerable<IdentityError>> RegisterWithAdminRole(ApiUserDto userDto);
		Task<IEnumerable<IdentityError>> RegisterWithUserRole(ApiUserDto userDto);
		Task<AuthResponseDto> Login(LoginDto loginDto);
		Task<string> CreateRefreshToken();
		Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto authResponseDto);
	}
}
