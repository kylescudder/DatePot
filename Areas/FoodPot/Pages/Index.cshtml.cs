using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static DatePot.Areas.FoodPot.Models.Food;
using DatePot.Areas.FoodPot.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace DatePot.Areas.FoodPot.Pages
{
    [ValidateAntiForgeryToken]
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IConfiguration _config;
		private readonly IFoodData _foodData;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		public IndexModel(ILogger<IndexModel> logger,
		IConfiguration config,
		IFoodData foodData,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData)
		{
			_logger = logger;
			_config = config;
			_foodData = foodData;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
		}
		public List<RestaurantList> Restaurants { get; set; }
		public List<SelectListItem> FoodType { get; set; }
		public List<SelectListItem> When { get; set; }
		public List<SelectListItem> Expense { get; set; }
		public List<SelectListItem> Location { get; set; }
		[BindProperty]
		public NewRestaurant NewRestaurant { get; set; }
		public NewFoodType NewFoodType { get; set; }
		public NewWhen NewWhen { get; set; }
		public List<RandomRestaurant> RandomRestaurants { get; set; }
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
				int index = PotAccess.FindIndex(item => item.PotID == 2);
				if (index == -1)
				{
					return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
				}
				Restaurants = _foodData.GetRestaurantList(UserGroupID).Result;
				var foodtypes = _foodData.GetFoodTypeList();
				var when = _foodData.GetWhenList();
				var expenses = _foodData.GetExpenseList();
				var locations = _foodData.GetLocationList();

				FoodType = new List<SelectListItem>();
				When = new List<SelectListItem>();
				Expense = new List<SelectListItem>();
				Location = new List<SelectListItem>();

				foodtypes.Result.ForEach(x =>
				{
					FoodType.Add(new SelectListItem { Value = x.FoodTypeID.ToString(), Text = x.FoodTypeText });
				});
				when.Result.ForEach(x =>
				{
					When.Add(new SelectListItem { Value = x.WhenID.ToString(), Text = x.WhenText });
				});
				expenses.Result.ForEach(x =>
				{
					Expense.Add(new SelectListItem { Value = x.ExpenseID.ToString(), Text = x.ExpenseText });
				});
				locations.Result.ForEach(x =>
				{
					Location.Add(new SelectListItem { Value = x.LocationID.ToString(), Text = x.LocationText });
				});

				return Page();
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
		public ActionResult OnPost(string RestaurantName, int ExpenseID, int LocationID, List<int> FoodType, List<int> When)
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
				int RestaurantID = _foodData.AddRestaurant(RestaurantName, ExpenseID, LocationID, UserGroupID).Result;

				foreach (var item in FoodType)
				{
					_foodData.AddRestaurantFoodType(RestaurantID, Convert.ToInt32(item));
				}
				foreach (var item in When)
				{
					_foodData.AddRestaurantWhen(RestaurantID, Convert.ToInt32(item));
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
		public async Task<IActionResult> OnPostFoodType()
		{
			try
			{
				if (!_foodData.FoodTypeDupeCheck(Request.Form["NewFoodType.FoodTypeText"].ToString()).Result)
				{
					await _foodData.AddFoodType(Request.Form["NewFoodType.FoodTypeText"].ToString());
					return RedirectToPage("./Index");
				}
				return RedirectToPage("./Index", new { @redirect = "FoodTypedupe", @value = Request.Form["NewFoodType.FoodTypeText"].ToString() });
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
		public async Task<IActionResult> OnPostWhen()
		{
			try
			{
				if (!_foodData.WhenDupeCheck(Request.Form["NewWhen.WhenText"].ToString()).Result)
				{
					await _foodData.AddWhen(Request.Form["NewWhen.WhenText"].ToString());
					return RedirectToPage("./Index");
				}
				return RedirectToPage("./Index", new { @redirect = "Whendupe", @value = Request.Form["NewWhen.WhenText"].ToString() });
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
		public async Task<IActionResult> OnPostRestaurantAttended()
		{
			try
			{
				return RedirectToPage("./Index", new { @redirect = "RestaurantAttended" });
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}
		public PartialViewResult OnGetRandomRestaurant(int WhenID)
		{
			try
			{
				int? UserGroupID = HttpContext.Session.GetInt32("UserGroupID");
				RandomRestaurants = _foodData.GetRandomRestaurant(WhenID, UserGroupID).Result;
				return new PartialViewResult
				{
					ViewName = "_RandomRestaurant",
					ViewData = new ViewDataDictionary<List<RandomRestaurant>>(ViewData, RandomRestaurants)
				};
			}
			catch (Exception ex)
			{
				Log.Error(ex.ToString());
				throw new Exception(ex.ToString());
			}
		}

	}
}
