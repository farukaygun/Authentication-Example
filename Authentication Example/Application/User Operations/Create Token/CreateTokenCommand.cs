using Authentication_Example.Entities;
using Authentication_Example.Token_Operations;
using Microsoft.AspNetCore.Identity;

namespace Authentication_Example.Application.User_Operations.Create_Token
{
	public class CreateTokenCommand(UserManager<User> userManager,
		IConfiguration configuration,
		CreateTokenModel model)
	{
		public async Task<TokenModel> Handle()
		{
			var user = userManager.Users.SingleOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);
			if (user is null)
				throw new InvalidOperationException("Username or password is wrong!");

			var tokenHandler = new TokenHandler(configuration);
			var token = tokenHandler.CreateAccessToken(user);

			user.RefreshToken = token.RefreshToken;
			user.RefreshTokenExpireDate = token.Expiration.AddMinutes(30);

			await userManager.UpdateAsync(user);

			return token;
		}
	}

	public class CreateTokenModel
	{
		public required string UserName { get; set; }
		public required string Password { get; set; }
	}
}
