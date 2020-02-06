using Microsoft.AspNetCore.Identity;

namespace ItSingular.RH.WebApi.Models
{
	public class ApplicationUser : IdentityUser
	{
		public int TenantId { get; set; }
	}

	public class User
	{
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class UserToRegister
	{
		public string UserName { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
	}

	public static class Roles
	{
		public const string ROLE_ADMINS = "Administradores";
		public const string ROLE_USERS = "Usuarios";
	}

	public class TokenConfigurations
	{
		public string Audience { get; set; }
		public string Issuer { get; set; }
		public int Seconds { get; set; }
	}
}
