using Authentication_Example.Db_Operations;
using Authentication_Example.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Example.Application.User_Operations.Create
{
	public class CreateUserCommand(AppDbContext context,
		IMapper mapper,
		UserManager<User> userManager,
		RoleManager<IdentityRole> roleManager,
		CreateUserModel model)
	{
		public async Task<CreateUserModel> Handle()
		{
			// Veritabanı sorgusu optimizasyonu
			var userEntity = await context.Users.Where(x => x.UserName == model.UserName && x.Email == model.Email).FirstOrDefaultAsync();

			if (userEntity != null)
				throw new InvalidOperationException("User already exist!");

			var newUser = mapper.Map<User>(model);

			// Kullanıcı oluşturma optimizasyonu
			var passwordHasher = new PasswordHasher<User>();
			var hashedPassword = passwordHasher.HashPassword(newUser, newUser.Password);
			newUser.PasswordHash = hashedPassword;
			newUser.Role = "User";

			var result = await userManager.CreateAsync(newUser);

			if (result.Succeeded)
			{
				if (!roleManager.RoleExistsAsync("User").Result)
					await roleManager.CreateAsync(new IdentityRole("User"));

				await userManager.AddToRoleAsync(newUser, "User");

				return model;
			}
			else throw new InvalidOperationException("User cannot be created!");
		}
	}

	public class CreateUserModel
	{
		public string? UserName { get; set; }
		public string? Password { get; set; }
		public string? FullName { get; set; }
		public string? Email { get; set; }
	}
}
