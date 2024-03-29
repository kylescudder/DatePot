using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using static DatePot.Areas.BeerPot.Models.Beers;
using DatePot.Areas.BeerPot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using Microsoft.AspNetCore.Http;
using DatePot.Areas.FilmPot.Data;
using Serilog;

namespace DatePot.Areas.BeerPot.Pages
{
    [ValidateAntiForgeryToken]
	public class ViewModel : PageModel
	{
		private readonly IConfiguration _config;
		private readonly IBeerData _BeerData;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		private readonly IFilmData _filmData;
		public ViewModel(IConfiguration config,
		IBeerData BeerData,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData,
		IFilmData filmData)
		{
			_config = config;
			_BeerData = BeerData;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
			_filmData = filmData;
		}
		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }
		[BindProperty]
		public UpdateBeerDetails UpdateBeerDetails { get; set; }

		public BeerDetails BeerDetails { get; set; }
		public List<BeerRatings> BeerRatingList { get; set; }
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
				BeerDetails = _BeerData.GetBeerDetails(Id).Result.FirstOrDefault();
				if (BeerDetails.UserGroupID != UserGroupID)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
				var users = _siteData.GetUserList(UserGroupID, 6);
				BeerRatingList = await _BeerData.GetBeerRatings(BeerDetails.BeerID);
				Users = new List<SelectListItem>();
				users.Result.ForEach(x =>
				{
					Users.Add(new SelectListItem { Value = x.UserID.ToString(), Text = x.UserName });
				});
				return Page();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPost(int BeerID, string BeerName, string Brewery)
		{
			try
			{
				string cs = _config.GetConnectionString("Default");
				await _BeerData.UpdateBeer(BeerID, BeerName, Brewery);
				return new JsonResult(BeerID);

			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
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
				await _BeerData.ArchiveBeer(UpdateBeerDetails.BeerID);

				return RedirectToPage("./Index");
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPostUpdateBeerRatings(string UserID, int Wankyness, int Taste, int BeerRatingID, int BeerID)
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
				await _BeerData.UpdateBeerRating(UserID, Wankyness, Taste, BeerRatingID, BeerID);
				result = new JsonResult(1);
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
