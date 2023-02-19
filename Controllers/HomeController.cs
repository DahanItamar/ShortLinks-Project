using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShortLinks.Models;
using System.Diagnostics;
using UrlProjectV1.Data;
using UrlProjectV1.Models;

namespace UrlProjectV1.Controllers
{
	public class HomeController : Controller
	{
		private readonly LinkDataBase _linkDataBase;
		private readonly UserManager<IdentityUser> _userManager;

		public HomeController(LinkDataBase linkDataBase, UserManager<IdentityUser> userManager)
		{
			_linkDataBase = linkDataBase;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult LinksByUser()
		{
			var userID = User.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;
			if (userID == null)
				return View("AccessDenied");
			List<Link> urls = _linkDataBase.AllLinksByID(userID).ToList();
			return View(urls);
		}

		public IActionResult LinkDetails(string shortURL)
		{
			var userID = User.Identity?.IsAuthenticated == true ? _userManager.GetUserId(User) : null;
			if (userID == null)
				return View("AccessDenied");
			var entries = _linkDataBase.GetLinkDetails(shortURL, userID);
			if (entries == null)
				return View("AccessDenied");
			return View(entries.ToList());
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
