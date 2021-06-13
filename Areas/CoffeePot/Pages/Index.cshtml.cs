using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
//using MySqlConnector;
using static DatePot.Areas.CoffeePot.Models.Coffees;
using DatePot.Areas.CoffeePot.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using Microsoft.AspNetCore.Http;
using DatePot.Areas.FilmPot.Data;

namespace DatePot.Areas.CoffeePot.Pages
{
	[ValidateAntiForgeryToken]
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IConfiguration _config;
		private readonly ICoffeeData _coffeeData;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		private readonly IFilmData _filmData;
		public IndexModel(ILogger<IndexModel> logger,
		IConfiguration config,
		ICoffeeData coffeeData,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData,
		IFilmData filmData)
		{
			_logger = logger;
			_config = config;
			_coffeeData = coffeeData;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
			_filmData = filmData;
		}
		public List<CoffeeList> Coffees { get; set; }
		public NewCoffee NewCoffee { get; set; }
		public List<RandomCoffee> RandomCoffees { get; set; }
		public List<PotAccess> PotAccess { get; set; }
		public List<CoffeeRatings> CoffeeRatingsList = new List<CoffeeRatings>();
		public List<UserList> Users = new List<UserList>();

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
				int index = PotAccess.FindIndex(item => item.PotID == 4);
				if (index == -1)
				{
					return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
				}
				Coffees = _coffeeData.GetCoffeeList(UserGroupID).Result;
				var users = _filmData.GetUserList(UserGroupID);
				users.Result.ForEach(x =>
				{
					Users.Add(x);
				});

				foreach (var item in Coffees)
				{
					var crl = _coffeeData.GetCoffeeRatings(item.CoffeeID).Result;
					foreach (var rating in crl)
					{
						CoffeeRatings cr = new CoffeeRatings();
						cr = rating;
						CoffeeRatingsList.Add(cr);
					}
				}
				return Page();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPost(string CoffeeName)
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
				int CoffeeID = _coffeeData.AddCoffee(CoffeeName, UserGroupID).Result;

				result = new JsonResult(CoffeeID);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
	}
}
