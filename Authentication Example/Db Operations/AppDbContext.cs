using Authentication_Example.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication_Example.Db_Operations
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
	{
		public override DbSet<User> Users{ get; set; }
	}
}
