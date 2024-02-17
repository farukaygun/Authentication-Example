using Authentication_Example.Application.User_Operations.Create;
using Authentication_Example.Application.User_Operations.Queris;
using Authentication_Example.Db_Operations;
using Authentication_Example.Entities;
using Authentication_Example.Token_Operations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Example.Controllers
{
	[Authorize(Roles = "User")]
	[ApiController]
	[Route("api/v1/[controller]s")]
	public class UserController(AppDbContext context,
		IConfiguration configuration,
		IMapper mapper,
		UserManager<User> userManager,
		RoleManager<IdentityRole> roleManager) : ControllerBase
	{
		[AllowAnonymous]
		[HttpPost]
		public IActionResult Create([FromBody] CreateUserModel user)
		{
			var command = new CreateUserCommand(context, mapper, userManager, roleManager, user);
			var result = command.Handle();

			if (result.IsCompletedSuccessfully)
				return Ok(result.Result);
  
			return BadRequest(result?.Exception?.Message);
		}

		[HttpGet]
		public IActionResult GetUsers()
		{
			var command = new GetUsersQuery(mapper, userManager);
			var result = command.Handle();

			if (result.IsCompletedSuccessfully)
			{
				return Ok(result.Result);
			}

			return BadRequest(result?.Exception?.Message);
		}

		[HttpGet("{id}")]
		public IActionResult GetById(string id)
		{
			var command = new GetByIdQuery(mapper, userManager, id);
			var result = command.Handle();

			if (result.IsCompletedSuccessfully)
			{
				return Ok(result.Result);
			}

			return BadRequest(result?.Exception?.Message);
		}

		[AllowAnonymous]
		[HttpPost("connect/token")]
		public async Task<IActionResult> Login([FromBody] UserLoginModel userLogin)
		{
			var user = await context.Users.SingleOrDefaultAsync(x => x.UserName == userLogin.UserName);

			if (user == null)
				return Unauthorized();

			var passwordHasher = new PasswordHasher<User>();
			var passwordVerified = passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, userLogin.Password);

			if (passwordVerified.Equals(0))
				return Unauthorized();

			var tokenHandler = new TokenHandler(configuration);
			var token = tokenHandler.CreateAccessToken(user);

			return Ok(token);
		}
	}

	public class UserLoginModel
	{
		public required string UserName { get; set; }
		public required string Password { get; set; }
	}
}
