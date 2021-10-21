using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using MySqlConnector;
using static DatePot.Areas.BeerPot.Models.Beers;
using DatePot.Areas.BeerPot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace DatePot.Areas.BeerPot.Pages
{
	[ValidateAntiForgeryToken]
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IConfiguration _config;
		private readonly IBeerData _BeerData;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		public IndexModel(ILogger<IndexModel> logger,
		IConfiguration config,
		IBeerData BeerData,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData)
		{
			_logger = logger;
			_config = config;
			_BeerData = BeerData;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
		}
		public List<BeerList> Beers { get; set; }
		public NewBeer NewBeer { get; set; }
		public List<RandomBeer> RandomBeers { get; set; }
		public List<PotAccess> PotAccess { get; set; }
		public async Task<ActionResult> OnGet()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToPage("/Account/Login", new { area = "Identity" });
			}
			try
			{
				int? UserGroupID = HttpContext.Session.GetInt32("UserGroupID");
				var user = await _userManager.GetUserAsync(User);
				PotAccess = await _siteData.GetPotAccess(user.Id.ToString(), UserGroupID);
				int index = PotAccess.FindIndex(item => item.PotID == 6);
				if (index == -1)
				{
					return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
				}
				Beers = _BeerData.GetBeerList(UserGroupID).Result;

				return Page();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPost(string BeerName, string Brewery)
		{
			try
			{
				int? UserGroupID = HttpContext.Session.GetInt32("UserGroupID");
				JsonResult result = null;
				if (ModelState.IsValid == false)
				{
					foreach (var modelStateKey in ViewData.ModelState.Keys)
					{
						var value = ViewData.ModelState[modelStateKey];
						foreach (var error in value.Errors)
						{
							var errorMessage = error.ErrorMessage;
							result = new JsonResult(modelStateKey + ": " + errorMessage);
						}
					}
					return result;
				}
				int BeerID = _BeerData.AddBeer(BeerName, Brewery, UserGroupID).Result;

				result = new JsonResult(BeerID);
				return result;
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
	}
}
