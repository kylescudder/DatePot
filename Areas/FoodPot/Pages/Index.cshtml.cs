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
using static DatePot.Areas.FoodPot.Models.Identity;
using DatePot.Areas.FoodPot.Data;

namespace DatePot.Areas.FoodPot.Pages
{
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
        [BindProperty]
        public NewRestaurant NewRestaurant { get; set; }
        public NewFoodType NewFoodType { get; set; }
        public NewWhen NewWhen { get; set; }
        public List<RandomRestaurant> RandomRestaurant { get; set; }
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

                FoodType = new List<SelectListItem>();
                When = new List<SelectListItem>();

                foodtypes.ForEach(x =>
                {
                    FoodType.Add(new SelectListItem { Value = x.FoodTypeID.ToString(), Text = x.FoodTypeText });
                });
                when.ForEach(x =>
                {
                    When.Add(new SelectListItem { Value = x.WhenID.ToString(), Text = x.WhenText });
                });

                RandomRestaurant = fd.GetRandomRestaurant(cs);

                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public ActionResult OnPost(string RestaurantName, List<int> FoodType, List<int> When)
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return Page();
                }
                string cs = _config.GetConnectionString("Default");
                int RestaurantID = fd.AddRestaurant(cs, RestaurantName);

                foreach (var item in FoodType)
                {
                    fd.AddRestaurantFoodType(cs, RestaurantID, Convert.ToInt32(item));
                }
                foreach (var item in When)
                {
                    fd.AddRestaurantWhen(cs, RestaurantID, Convert.ToInt32(item));
                }

                JsonResult result = null;
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
        public async Task<IActionResult> OnPostAddRestaurant()
        {
            try
            {
                string cs = _config.GetConnectionString("Default");

                fd.RestaurantWatched(cs, Convert.ToInt32(Request.Form["RestaurantID"]));

                //return RedirectToPage("./Index", new { @redirect = "Whendupe", @value = Request.Form["NewWhen.WhenText"].ToString() });

                return RedirectToPage("./Index", new { @redirect = "RestaurantWatched" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
