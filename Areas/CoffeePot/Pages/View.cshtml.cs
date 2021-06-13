using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using static DatePot.Areas.CoffeePot.Models.Coffees;
using DatePot.Areas.CoffeePot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using Microsoft.AspNetCore.Http;
using DatePot.Areas.FilmPot.Data;

namespace DatePot.Areas.CoffeePot.Pages
{
    [ValidateAntiForgeryToken]
	public class ViewModel : PageModel
	{
		private readonly IConfiguration _config;
		private readonly ICoffeeData _CoffeeData;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		private readonly IFilmData _filmData;
		public ViewModel(IConfiguration config,
		ICoffeeData CoffeeData,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData,
		IFilmData filmData)
		{
			_config = config;
			_CoffeeData = CoffeeData;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
			_filmData = filmData;
		}
		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }
		[BindProperty]
		public UpdateCoffeeDetails UpdateCoffeeDetails { get; set; }

		public CoffeeDetails CoffeeDetails { get; set; }
		public List<CoffeeRatings> CoffeeRatingList { get; set; }
		public List<PotAccess> PotAccess { get; set; }
		public List<SelectListItem> Users { get; set; }
		public async Task<IActionResult> OnGet()
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
				string cs = _config.GetConnectionString("Default");
				CoffeeDetails = _CoffeeData.GetCoffeeDetails(Id).Result.FirstOrDefault();
				if (CoffeeDetails.UserGroupID != UserGroupID)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
				var users = _filmData.GetUserList(UserGroupID);
				CoffeeRatingList = await _CoffeeData.GetCoffeeRatings(CoffeeDetails.CoffeeID);
				Users = new List<SelectListItem>();
				users.Result.ForEach(x =>
				{
					Users.Add(new SelectListItem { Value = x.UserID.ToString(), Text = x.UserName });
				});
				return Page();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
		public async Task<IActionResult> OnPost()
		{
			try
			{
				string cs = _config.GetConnectionString("Default");
				await _CoffeeData.UpdateCoffee(Convert.ToInt16(Request.Form["UpdateCoffeeDetails.CoffeeID"]),
					Request.Form["UpdateCoffeeDetails.CoffeeName"].ToString());
				return RedirectToPage("./View", new { @Id = Request.Form["UpdateCoffeeDetails.CoffeeID"].ToString(), @redirect = "update" });
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
		public async Task<IActionResult> OnPostArchive()
		{
			try
			{
				if (ModelState.IsValid == false)
				{
					return Page();
				}
				string cs = _config.GetConnectionString("Default");
				await _CoffeeData.ArchiveCoffee(UpdateCoffeeDetails.CoffeeID);

				return RedirectToPage("./Index");
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPostUpdateCoffeeRatings(string UserID, int? Experience, int? Taste, int CoffeeRatingID, int CoffeeID)
		{
			try
			{
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
				await _CoffeeData.UpdateCoffeeRating(UserID, Experience, Taste, CoffeeRatingID, CoffeeID);
				result = new JsonResult(1);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}

	}
}
