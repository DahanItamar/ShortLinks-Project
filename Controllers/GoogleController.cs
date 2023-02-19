#pragma warning disable CS8625
#pragma warning disable CS8603
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace UrlProjectV1.Controllers
{
	public class GoogleController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;

		public GoogleController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[AllowAnonymous]
		public IActionResult Login(string returnUrl = "/")
		{
			var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", Url.Action("Callback", "Google", new { returnUrl }));
			return Challenge(properties, "Google");
		}

		[AllowAnonymous]
		public async Task<IActionResult> Callback(string returnUrl = "/")
		{
			var info = await _signInManager.GetExternalLoginInfoAsync();
			if (info == null)
				return RedirectToAction("Login");
			var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
			if (result.Succeeded)
			{
				var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
				if (user != null)
				{
					var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
					await HttpContext.SignInAsync(claimsPrincipal);
				}
				return LocalRedirect(returnUrl);
			}
			else
			{
				var email = info.Principal.FindFirstValue(ClaimTypes.Email);
				if (email != null)
				{
					var user = await _userManager.FindByEmailAsync(email);
					if (user == null)
					{
						user = new IdentityUser
						{
							UserName = email,
							Email = email,
						};
						var createResult = await _userManager.CreateAsync(user);
						if (!createResult.Succeeded)
							return BadRequest();
					}

					var addLoginResult = await _userManager.AddLoginAsync(user, info);
					if (addLoginResult.Succeeded)
					{
						await _signInManager.SignInAsync(user, isPersistent: false);
						var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
						await HttpContext.SignInAsync(claimsPrincipal);
						return LocalRedirect(returnUrl);
					}
				}
				return BadRequest();
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
			return RedirectToAction("Index", "Home");
		}
	}
}
