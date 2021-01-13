using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using static DatePot.Areas.FoodPot.Models.Food;
using DatePot.Areas.FoodPot.Data;

namespace DatePot.Areas.FoodPot.Pages
{
    [ValidateAntiForgeryToken]
    public class ViewModel : PageModel
    {
        private readonly IConfiguration _config;
        public ViewModel(IConfiguration config)
        {
            _config = config;
        }
        FoodData fd = new FoodData();

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
        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
                string cs = _config.GetConnectionString("Default");
                RestaurantDetails = fd.GetRestaurantDetails(cs, Id).FirstOrDefault();
                if (RestaurantDetails != null)
                {
                    if (RestaurantDetails != null)
                    {
                        var foodtypes = fd.GetFoodTypeList(cs);
                        var when = fd.GetWhenList(cs);
                        var expenses = fd.GetExpenseList(cs);
                        var locations = fd.GetLocationList(cs);

                        //FoodType = FoodTypes.Where(x => x.FoodTypeID == RestaurantDetails.FoodTypeID).FirstOrDefault()?.FoodTypeText;

                        FoodTypes = new List<SelectListItem>();
                        When = new List<SelectListItem>();
                        Expenses = new List<SelectListItem>();
                        Locations = new List<SelectListItem>();
                        RestaurantFoodTypes = new List<RestaurantFoodTypes>();
                        RestaurantWhen = new List<RestaurantWhens>();

                        RestaurantFoodTypes = fd.GetRestaurantFoodTypes(cs, Id);
                        RestaurantWhen = fd.GetRestaurantWhens(cs, Id);

                        foodtypes.ForEach(x =>
                        {
                            FoodTypes.Add(new SelectListItem { Value = x.FoodTypeID.ToString(), Text = x.FoodTypeText });
                        });
                        when.ForEach(x =>
                        {
                            When.Add(new SelectListItem { Value = x.WhenID.ToString(), Text = x.WhenText });
                        });
                        expenses.ForEach(x =>
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
                        locations.ForEach(x =>
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
                string cs = _config.GetConnectionString("Default");
                fd.UpdateRestaurant(cs, RestaurantID, RestaurantName, ExpenseID, LocationID);

                foreach (var item in FoodType)
                {
                    if (item != 0)
                    {
                        fd.AddRestaurantFoodType(cs, RestaurantID, Convert.ToInt32(item));
                    }

                }
                foreach (var item in When)
                {
                    if (item != 0)
                    {
                        fd.AddRestaurantWhen(cs, RestaurantID, Convert.ToInt32(item));
                    }

                }

                result = new JsonResult(RestaurantID);
                return result;
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
                fd.ArchiveRestaurant(cs, UpdateRestaurantDetails.RestaurantID);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
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
                string cs = _config.GetConnectionString("Default");
                fd.DeleteRestaurantFoodType(cs, RestaurantFoodTypeID);

                result = new JsonResult(RestaurantID);
                return result;
            }
            catch (Exception ex)
            {
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
                string cs = _config.GetConnectionString("Default");
                fd.DeleteRestaurantWhen(cs, RestaurantWhenID);

                result = new JsonResult(RestaurantID);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
