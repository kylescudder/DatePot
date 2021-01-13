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
using MySqlConnector;
using static DatePot.Areas.FoodPot.Models.Food;
using DatePot.Areas.FoodPot.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DatePot.Areas.FoodPot.Pages
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        FoodData fd = new FoodData();
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
        public ActionResult OnGet()
         {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
                string cs = _config.GetConnectionString("Default");
                Restaurants = fd.GetRestaurantList(cs);
                var foodtypes = fd.GetFoodTypeList(cs);
                var when = fd.GetWhenList(cs);
                var expenses = fd.GetExpenseList(cs);
                var locations = fd.GetLocationList(cs);

                FoodType = new List<SelectListItem>();
                When = new List<SelectListItem>();
                Expense = new List<SelectListItem>();
                Location = new List<SelectListItem>();

                foodtypes.ForEach(x =>
                {
                    FoodType.Add(new SelectListItem { Value = x.FoodTypeID.ToString(), Text = x.FoodTypeText });
                });
                when.ForEach(x =>
                {
                    When.Add(new SelectListItem { Value = x.WhenID.ToString(), Text = x.WhenText });
                });
                expenses.ForEach(x =>
                {
                    Expense.Add(new SelectListItem { Value = x.ExpenseID.ToString(), Text = x.ExpenseText });
                });
                locations.ForEach(x =>
                {
                    Location.Add(new SelectListItem { Value = x.LocationID.ToString(), Text = x.LocationText });
                });

                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public ActionResult OnPost(string RestaurantName, int ExpenseID, int LocationID, List<int> FoodType, List<int> When)
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
                string cs = _config.GetConnectionString("Default");
                int RestaurantID = fd.AddRestaurant(cs, RestaurantName, ExpenseID, LocationID);

                foreach (var item in FoodType)
                {
                    fd.AddRestaurantFoodType(cs, RestaurantID, Convert.ToInt32(item));
                }
                foreach (var item in When)
                {
                    fd.AddRestaurantWhen(cs, RestaurantID, Convert.ToInt32(item));
                }

                result = new JsonResult(RestaurantID);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<IActionResult> OnPostFoodType()
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                if (!fd.FoodTypeDupeCheck(cs, Request.Form["NewFoodType.FoodTypeText"].ToString()))
                {
                    fd.AddFoodType(cs, Request.Form["NewFoodType.FoodTypeText"].ToString());
                    return RedirectToPage("./Index");
                }
                return RedirectToPage("./Index", new { @redirect = "FoodTypedupe", @value = Request.Form["NewFoodType.FoodTypeText"].ToString() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<IActionResult> OnPostWhen()
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                if (!fd.WhenDupeCheck(cs, Request.Form["NewWhen.WhenText"].ToString()))
                {
                    fd.AddWhen(cs, Request.Form["NewWhen.WhenText"].ToString());
                    return RedirectToPage("./Index");
                }
                return RedirectToPage("./Index", new { @redirect = "Whendupe", @value = Request.Form["NewWhen.WhenText"].ToString() });
            }
            catch (Exception ex)
            {
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
                throw new Exception(ex.ToString());
            }
        }
        public PartialViewResult OnGetRandomRestaurant(int WhenID)
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                RandomRestaurants = fd.GetRandomRestaurant(cs, WhenID);
                return new PartialViewResult
                {
                    ViewName = "_RandomRestaurant",
                    ViewData = new ViewDataDictionary<List<RandomRestaurant>>(ViewData, RandomRestaurants)
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
