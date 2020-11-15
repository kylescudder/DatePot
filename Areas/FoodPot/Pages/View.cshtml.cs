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
using static DatePot.Areas.FoodPot.Models.Identity;
using DatePot.Areas.FoodPot.Data;

namespace DatePot.Areas.FoodPot.Pages
{
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
        public List<SelectListItem> FoodTypes { get; set; }
        public List<SelectListItem> When { get; set; }
        public List<SelectListItem> Users { get; set; }
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

                        //FoodType = FoodTypes.Where(x => x.FoodTypeID == RestaurantDetails.FoodTypeID).FirstOrDefault()?.FoodTypeText;

                        FoodTypes = new List<SelectListItem>();
                        When = new List<SelectListItem>();

                        foodtypes.ForEach(x =>
                        {
                            FoodTypes.Add(new SelectListItem { Value = x.FoodTypeID.ToString(), Text = x.FoodTypeText });
                        });
                        when.ForEach(x =>
                        {
                            When.Add(new SelectListItem { Value = x.WhenID.ToString(), Text = x.WhenText });
                        });

                        //FoodTypes.ForEach(x =>
                        //{
                        //    if (RestaurantDetails.FoodTypeID == x.FoodTypeID)
                        //    {
                        //        FoodTypes.Add(new SelectListItem { Value = x.FoodTypeID.ToString(), Text = x.FoodTypeText, Selected = true });
                        //    }
                        //    else
                        //    {
                        //        FoodTypes.Add(new SelectListItem { Value = x.FoodTypeID.ToString(), Text = x.FoodTypeText });
                        //    }
                        //});
                        //Whens.ForEach(x =>
                        //{
                        //    if (RestaurantDetails.WhenID == x.WhenID)
                        //    {
                        //        Whens.Add(new SelectListItem { Value = x.WhenID.ToString(), Text = x.WhenName, Selected = true });
                        //    }
                        //    else
                        //    {
                        //        Whens.Add(new SelectListItem { Value = x.WhenID.ToString(), Text = x.WhenName });
                        //    }
                        //});
                    }
                }

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
                if (ModelState.IsValid == false)
                {
                    return Page();
                }
                string cs = _config.GetConnectionString("Default");
                fd.UpdateRestaurant(cs, UpdateRestaurantDetails);

                return RedirectToPage("./View", new { @Id = UpdateRestaurantDetails.RestaurantID, @redirect = "update" });
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
    }
}
