﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using UrlProjectV1.Models;

namespace UrlProjectV1.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly UserManager<IdentityUser> _userManager;

		public HomeController(ILogger<HomeController> logger, UserManager<IdentityUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> LinksByUser()
		{
			using (HttpClient client = new HttpClient())
			{
				var userID = User.Identity.IsAuthenticated ? _userManager.GetUserId(User) : null;
				if (userID == null)
					return View("AccessDenied");
				HttpResponseMessage response = await client.GetAsync($"{HttpContext.FullDomaine()}/W/Links?userID={userID}");
				if (response.IsSuccessStatusCode)
				{
					string json = await response.Content.ReadAsStringAsync();
					List<Link> urls = JsonConvert.DeserializeObject<List<Link>>(json);
					return View(urls);
				}
				else
					return View("Error");
			}
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}