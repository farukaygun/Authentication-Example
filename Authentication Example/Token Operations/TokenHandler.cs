using Authentication_Example.Controllers;
using Authentication_Example.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication_Example.Token_Operations
{
	public class TokenHandler(IConfiguration configuration)
	{

		public TokenModel CreateAccessToken(User user)
		{
			var tokenModel = new TokenModel();
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"]!));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			var claims = new List<Claim>
			{
				new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
				new(JwtRegisteredClaimNames.Email, user.Email!),
				new(ClaimTypes.Name, user.UserName!),
				new(ClaimTypes.Role, user.Role!)
			};	

			tokenModel.Expiration = DateTime.Now.AddMinutes(15);
			JwtSecurityToken securityToken = new JwtSecurityToken(
				issuer: configuration["Token:Issuer"],
				audience: configuration["Token:Audience"],
				claims: claims,
				expires: tokenModel.Expiration,
				notBefore: DateTime.Now,
				signingCredentials: credentials
			);

			var tokenHandler = new JwtSecurityTokenHandler();

			tokenModel.AccessToken = tokenHandler.WriteToken(securityToken);
			tokenModel.RefreshToken = CreateRefreshToken();

			return tokenModel;
		}

		private static string CreateRefreshToken() => Guid.NewGuid().ToString();

	}

	public class TokenModel
	{
		public string? AccessToken { get; set; }
		public DateTime Expiration { get; set; }
		public string? RefreshToken { get; set; }
	}
}

