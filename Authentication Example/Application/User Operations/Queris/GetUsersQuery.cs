using Authentication_Example.Db_Operations;
using Authentication_Example.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Example.Application.User_Operations.Queris
{
	public class GetUsersQuery(IMapper mapper, UserManager<User> userManager)
	{
		public async Task<List<GetUserDto>> Handle()
		{
			var users = await userManager.Users.ToListAsync();
			var userList = mapper.Map<List<User>, List<GetUserDto>>(users);

			return userList;
		}
	}

	public class GetUserDto
	{
		public string? Id { get; set; }
		public string? UserName { get; set; }
		public string? FullName { get; set; }
		public string? Email { get; set; }
	}
}
