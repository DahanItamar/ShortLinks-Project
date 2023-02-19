using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
			var userID = User.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;
			if (!Uri.TryCreate(originalUrl, UriKind.Absolute, out var uri) ||
				(uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
				return BadRequest("Error: Invalid URL");
			return $"{HttpContext.FullDomaine()}/W/{await _linkDataBase.CreateShortLink(originalUrl, userID)}";
		}

		[HttpGet("/W/{shortURL}")]
		public async Task<ActionResult> Result(string shortURL)
		{
			if (string.IsNullOrEmpty(shortURL) || shortURL.Length != 7)
				return BadRequest();
			var resultURL = await _linkDataBase.GiveOriginalLink(shortURL, HttpContext.ClientIp());
			if (resultURL != null)
				return Redirect(resultURL);
			return NotFound();
		}

		// The caller's identity comes from the session — a user can only ever
		// read their own links and click logs.
		[Authorize]
		[HttpGet("/W/Links")]
		public IActionResult LinksByUser()
			=> Ok(_linkDataBase.AllLinksByID(_userManager.GetUserId(User)!));

		[Authorize]
		[HttpGet("/W/Entries")]
		public IActionResult LinkDetails(string shortURL)
		{
			var entries = _linkDataBase.GetLinkDetails(shortURL, _userManager.GetUserId(User)!);
			if (entries == null)
				return NotFound();
			return Ok(entries);
		}
	}
}
