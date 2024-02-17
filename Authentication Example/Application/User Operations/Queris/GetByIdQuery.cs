using Authentication_Example.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Authentication_Example.Application.User_Operations.Queris
{
	public class GetByIdQuery(IMapper mapper, 
		UserManager<User> userManager,
		string id)
	{
		public async Task<GetUserDto> Handle()
		{
			var user = await userManager.FindByIdAsync(id);
			
			return user == null ? 
				throw new InvalidOperationException("User not found!") : 
				mapper.Map<GetUserDto>(user);
		}
	}
}
