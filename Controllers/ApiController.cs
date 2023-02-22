#pragma warning disable CS8625
#pragma warning disable CS8603
#pragma warning disable CS8600
#pragma warning disable CS8602
#pragma warning disable CS8604
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShortLinks.Models;
using System.Drawing;
using UrlProjectV1.Data;

namespace UrlProjectV1.Controllers
{
	[Route("Api")]
	[ApiController]
	public class ApiController : ControllerBase
	{
		private readonly LinkDataBase _linkDataBase;
		private readonly UserManager<IdentityUser> _userManager;

		public ApiController(LinkDataBase urlDataBase, UserManager<IdentityUser> userManager)
		{
			_linkDataBase = urlDataBase;
			_userManager = userManager;
		}

		[HttpPost("cutter")]
		public async Task<ActionResult<string>> CreateShortURL([FromBody] string originalUrl)
		{
			var userID = User.Identity.IsAuthenticated ? _userManager.GetUserId(User) : null;
			if (!Uri.TryCreate(originalUrl, UriKind.Absolute, out Uri uri) ||
				(uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
				return BadRequest("Error: Invalid URL");
			return $"{HttpContext.FullDomaine()}/W/{await _linkDataBase.CreateShortLink(originalUrl, userID)}";
		}

		[HttpGet("/W/{shortURL}")]
		public async Task<ActionResult> Result(string shortURL)
		{
			if (String.IsNullOrEmpty(shortURL) || shortURL.Count() != 7)
				return BadRequest();
			var resultURL = await _linkDataBase.GiveOriginalLink(shortURL);
			if (resultURL != null)
				return Redirect(resultURL);
			return BadRequest();
		}

		[HttpGet("/W/Links")]
		public IActionResult LinksByUser(string userID)
			=> Ok(_linkDataBase.AllLinksByID(userID));

		[HttpGet("/W/Entries")]
		public IActionResult LinkDetails(string shortURL)
			=> Ok(_linkDataBase.GetLinkDetails(shortURL));
	}
}
