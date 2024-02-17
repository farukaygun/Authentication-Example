using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication_Example.Entities
{
	public class User : IdentityUser
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public override required string Id { get; set; }
		public required string Password { get; set; }
		public required string FullName { get; set; }
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpireDate { get; set; }
		public string? Role { get; set; }
	}
}
