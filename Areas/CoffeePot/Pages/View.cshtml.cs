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
using static DatePot.Areas.CoffeePot.Models.Coffees;
using DatePot.Areas.CoffeePot.Data;

namespace DatePot.Areas.CoffeePot.Pages
{
    public class ViewModel : PageModel
    {
        private readonly IConfiguration _config;
        public ViewModel(IConfiguration config)
        {
            _config = config;
        }
        CoffeeData fd = new CoffeeData();
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public UpdateCoffeeDetails UpdateCoffeeDetails { get; set; }

        public CoffeeDetails CoffeeDetails { get; set; }
        //public string Genre { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
                string cs = _config.GetConnectionString("Default");
                CoffeeDetails = fd.GetCoffeeDetails(cs, Id).FirstOrDefault();

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
                    fd.UpdateCoffee(cs, Convert.ToInt16(Request.Form["UpdateCoffeeDetails.CoffeeID"]),
                    Request.Form["UpdateCoffeeDetails.CoffeeName"].ToString(),
                    Convert.ToInt16(Request.Form["UpdateCoffeeDetails.KyleTaste"]),
                    Convert.ToInt16(Request.Form["UpdateCoffeeDetails.RhiannTaste"]),
                    Convert.ToInt16(Request.Form["UpdateCoffeeDetails.KyleExperience"]),
                    Convert.ToInt16(Request.Form["UpdateCoffeeDetails.RhiannExperience"]));
                return RedirectToPage("./View", new { @Id = Convert.ToInt16(Request.Form["UpdateCoffeeDetails.CoffeeID"]), @redirect = "update"  });
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
                fd.ArchiveCoffee(cs, UpdateCoffeeDetails.CoffeeID);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
