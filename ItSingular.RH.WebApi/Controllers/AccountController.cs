using ItSingular.RH.WebApi.Helpers;
using ItSingular.RH.WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ItSingular.RH.WebApi.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
		[AllowAnonymous]
		[HttpPost("Login")]
		public async Task<IActionResult> AccountLogin(
			[FromBody]User usuario,
			[FromServices]UserManager<ApplicationUser> userManager,
			[FromServices]SignInManager<ApplicationUser> signInManager,
			[FromServices]SigningConfigurations signingConfigurations,
			[FromServices]TokenConfigurations tokenConfigurations)
		{
			bool credenciaisValidas = false;
			if (usuario != null && !String.IsNullOrWhiteSpace(usuario.UserName))
			{
				// Verifica a existência do usuário nas tabelas do
				// ASP.NET Core Identity
				var userIdentity = await userManager.FindByNameAsync(usuario.UserName);
				if (userIdentity != null)
				{
					// Efetua o login com base no Id do usuário e sua senha
					var resultadoLogin = await signInManager.CheckPasswordSignInAsync(userIdentity, usuario.Password, false);
					if (resultadoLogin.Succeeded)
					{
						// Verifica se o usuário em questão possui
						// a permissão de acesso
						bool userIsInRoleAdmins = await userManager.IsInRoleAsync(userIdentity, Roles.ROLE_ADMINS);
						bool userIsInRoleUser = await userManager.IsInRoleAsync(userIdentity, Roles.ROLE_USERS);
						credenciaisValidas = userIsInRoleAdmins || userIsInRoleUser;
					}
				}
			}

			if (credenciaisValidas)
			{
				ClaimsIdentity identity = new ClaimsIdentity(
					new GenericIdentity(usuario.UserName, "Login"),
					new[] {
						new Claim(ClaimTypes.Role, Roles.ROLE_ADMINS),
						new Claim(ClaimTypes.Role, Roles.ROLE_USERS),
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
						new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName)
					}
				);

				DateTime dataCriacao = DateTime.Now;
				DateTime dataExpiracao = dataCriacao +
					TimeSpan.FromSeconds(tokenConfigurations.Seconds);

				var handler = new JwtSecurityTokenHandler();
				var securityToken = handler.CreateToken(new SecurityTokenDescriptor
				{
					Issuer = tokenConfigurations.Issuer,
					Audience = tokenConfigurations.Audience,
					SigningCredentials = signingConfigurations.SigningCredentials,
					Subject = identity,
					NotBefore = dataCriacao,
					Expires = dataExpiracao
				});
				var token = handler.WriteToken(securityToken);

				return Ok(new
				{
					authenticated = true,
					created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
					expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
					accessToken = token,
					message = "OK"
				});
			}
			else
			{
				return Unauthorized();
			}
		}

		[HttpPost("register")]
		[AllowAnonymous]
		public async Task<IActionResult> AccountRegister(
			[FromBody]UserToRegister usuario,
			[FromServices]UserManager<ApplicationUser> userManager,
			[FromServices]SignInManager<ApplicationUser> signInManager,
			[FromServices]SigningConfigurations signingConfigurations,
			[FromServices]TokenConfigurations tokenConfigurations)
		{
			var user = new ApplicationUser { UserName = usuario.UserName, Email = usuario.Email };
			//usuario.Password = string.IsNullOrEmpty(usuario.Password) ? "DefaultPassword00@sinistro" : usuario.Password;
			var result = await userManager.CreateAsync(user, usuario.Password);

			if (result.Succeeded)
			{
				userManager.AddToRoleAsync(user, usuario.Role).Wait();

				ClaimsIdentity identity = new ClaimsIdentity(
					new GenericIdentity(usuario.UserName, "Login"),
					new[] {
						new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
						new Claim(JwtRegisteredClaimNames.UniqueName, usuario.UserName)
					}
				);

				DateTime dataCriacao = DateTime.Now;
				DateTime dataExpiracao = dataCriacao +
					TimeSpan.FromSeconds(tokenConfigurations.Seconds);

				var handler = new JwtSecurityTokenHandler();
				var securityToken = handler.CreateToken(new SecurityTokenDescriptor
				{
					Issuer = tokenConfigurations.Issuer,
					Audience = tokenConfigurations.Audience,
					SigningCredentials = signingConfigurations.SigningCredentials,
					Subject = identity,
					NotBefore = dataCriacao,
					Expires = dataExpiracao
				});
				var token = handler.WriteToken(securityToken);

				return Ok(
					new
					{
						authenticated = true,
						created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
						expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
						accessToken = token,
						message = "OK"
					});
			}
			else
			{
				return BadRequest(
				new
				{
					authenticated = false,
					message = "Falha ao registrar usuário"
				});
			}
		}
	}
}