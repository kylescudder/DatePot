using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
//using MySql.Data.MySqlClient;
using static DatePot.Areas.FoodPot.Models.Food;
using DatePot.Areas.FoodPot.Data;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace DatePot.Areas.FoodPot.Pages
{
    [ValidateAntiForgeryToken]
	public class ViewModel : PageModel
	{
		private readonly IConfiguration _config;
		private readonly IFoodData _foodData;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		public ViewModel(IConfiguration config,
		IFoodData foodData,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData)
		{
			_config = config;
			_foodData = foodData;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
		}
		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }
		[BindProperty]
		public UpdateRestaurantDetails UpdateRestaurantDetails { get; set; }

		public RestaurantDetails RestaurantDetails { get; set; }
		//public string FoodType { get; set; }
		public List<RestaurantFoodTypes> RestaurantFoodTypes { get; set; }
		public List<RestaurantWhens> RestaurantWhen { get; set; }
		public List<SelectListItem> FoodTypes { get; set; }
		public List<SelectListItem> When { get; set; }
		public List<SelectListItem> Expenses { get; set; }
		public List<SelectListItem> Locations { get; set; }
		public List<PotAccess> PotAccess { get; set; }
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
				int index = PotAccess.FindIndex(item => item.PotID == 2);
				if (index == -1)
				{
					return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
				}
				RestaurantDetails = _foodData.GetRestaurantDetails(Id).Result.FirstOrDefault();
				if (RestaurantDetails.UserGroupID != UserGroupID)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
				if (RestaurantDetails != null)
				{
					if (RestaurantDetails != null)
					{
						var foodtypes = _foodData.GetFoodTypeList();
						var when = _foodData.GetWhenList();
						var expenses = _foodData.GetExpenseList();
						var locations = _foodData.GetLocationList();

						FoodTypes = new List<SelectListItem>();
						When = new List<SelectListItem>();
						Expenses = new List<SelectListItem>();
						Locations = new List<SelectListItem>();
						RestaurantFoodTypes = new List<RestaurantFoodTypes>();
						RestaurantWhen = new List<RestaurantWhens>();

						RestaurantFoodTypes = _foodData.GetRestaurantFoodTypes(Id).Result;
						RestaurantWhen = _foodData.GetRestaurantWhens(Id).Result;

						foodtypes.Result.ForEach(x =>
						{
							FoodTypes.Add(new SelectListItem { Value = x.FoodTypeID.ToString(), Text = x.FoodTypeText });
						});
						when.Result.ForEach(x =>
						{
							When.Add(new SelectListItem { Value = x.WhenID.ToString(), Text = x.WhenText });
						});
						expenses.Result.ForEach(x =>
						{
							if (RestaurantDetails.ExpenseID == x.ExpenseID)
							{
								Expenses.Add(new SelectListItem { Value = x.ExpenseID.ToString(), Text = x.ExpenseText, Selected = true });
							}
							else
							{
								Expenses.Add(new SelectListItem { Value = x.ExpenseID.ToString(), Text = x.ExpenseText });
							}
						});
						locations.Result.ForEach(x =>
						{
							if (RestaurantDetails.LocationID == x.LocationID)
							{
								Locations.Add(new SelectListItem { Value = x.LocationID.ToString(), Text = x.LocationText, Selected = true });
							}
							else
							{
								Locations.Add(new SelectListItem { Value = x.LocationID.ToString(), Text = x.LocationText });
							}
						});

					}
				}

				return Page();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPost(int RestaurantID, string RestaurantName, int ExpenseID, int LocationID, List<int> FoodType, List<int> When)
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
				await _foodData.UpdateRestaurant(RestaurantID, RestaurantName, ExpenseID, LocationID);

				foreach (var item in FoodType)
				{
					if (item != 0)
					{
						await _foodData.AddRestaurantFoodType(RestaurantID, Convert.ToInt32(item));
					}

				}
				foreach (var item in When)
				{
					if (item != 0)
					{
						await _foodData.AddRestaurantWhen(RestaurantID, Convert.ToInt32(item));
					}

				}

				result = new JsonResult(RestaurantID);
				return result;
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
				await _foodData.ArchiveRestaurant(UpdateRestaurantDetails.RestaurantID);

				return RedirectToPage("./Index");
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPostDeleteFoodType(int RestaurantFoodTypeID, int RestaurantID)
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
				await _foodData.DeleteRestaurantFoodType(RestaurantFoodTypeID);

				result = new JsonResult(RestaurantID);
				return result;
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPostDeleteWhen(int RestaurantWhenID, int RestaurantID)
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
				await _foodData.DeleteRestaurantWhen(RestaurantWhenID);

				result = new JsonResult(RestaurantID);
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
