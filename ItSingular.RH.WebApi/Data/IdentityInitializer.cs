using ItSingular.RH.WebApi.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace ItSingular.RH.WebApi.Data
{
	public class IdentityInitializer
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;


		public IdentityInitializer(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public void Initialize()
		{

			if (_context.Database.EnsureCreated())
			{

				if (!_roleManager.RoleExistsAsync(Roles.ROLE_ADMINS).Result)
				{
					var resultado = _roleManager.CreateAsync(
						new IdentityRole(Roles.ROLE_ADMINS)).Result;
					if (!resultado.Succeeded)
					{
						throw new Exception(
							$"Erro durante a criação da role {Roles.ROLE_ADMINS}.");
					}
				}

				if (!_roleManager.RoleExistsAsync(Roles.ROLE_USERS).Result)
				{
					var resultado = _roleManager.CreateAsync(
						new IdentityRole(Roles.ROLE_USERS)).Result;
					if (!resultado.Succeeded)
					{
						throw new Exception(
							$"Erro durante a criação da role {Roles.ROLE_USERS}.");
					}
				}

				CreateUser(
						new ApplicationUser()
						{
							UserName = "Administrador01",
							Email = "admin01@itsingular.com.br",
							EmailConfirmed = true
						}, "Admin01@sinistro", Roles.ROLE_ADMINS);

				CreateUser(
					new ApplicationUser()
					{
						UserName = "Usuario01",
						Email = "usuario01@itsingular.com.br",
						EmailConfirmed = true
					}, "Usuario01@sinistro", Roles.ROLE_USERS);

			}

		}

		private void CreateUser(
			ApplicationUser user,
			string password,
			string initialRole = null)
		{
			if (_userManager.FindByNameAsync(user.UserName).Result == null)
			{
				var resultado = _userManager
					.CreateAsync(user, password).Result;

				if (resultado.Succeeded &&
					!String.IsNullOrWhiteSpace(initialRole))
				{
					_userManager.AddToRoleAsync(user, initialRole).Wait();
				}
			}
		}
	}
}
