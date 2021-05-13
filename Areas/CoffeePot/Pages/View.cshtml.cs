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
//using MySql.Data.MySqlClient;
using static DatePot.Areas.CoffeePot.Models.Coffees;
using DatePot.Areas.CoffeePot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;

namespace DatePot.Areas.CoffeePot.Pages
{
    [ValidateAntiForgeryToken]
    public class ViewModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly ICoffeeData _coffeeData;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityData _identityData;
        private readonly ISiteData _siteData;
        public ViewModel(IConfiguration config, 
        ICoffeeData coffeeData,
        UserManager<IdentityUser> userManager,
        IIdentityData identityData,
        ISiteData siteData)
        {
            _config = config;
            _coffeeData = coffeeData;
            _userManager = userManager;
            _identityData = identityData;
            _siteData = siteData;
        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public UpdateCoffeeDetails UpdateCoffeeDetails { get; set; }

        public CoffeeDetails CoffeeDetails { get; set; }
        public List<PotAccess> PotAccess { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
				var user = await _userManager.GetUserAsync(User);
				var UserID = _identityData.GetUser(user.Id.ToString()).Result.FirstOrDefault().UserID.ToString();
				PotAccess = await _siteData.GetPotAccess(Convert.ToInt32(UserID));
				int index = PotAccess.FindIndex(item => item.PotID == 4);
                if (index == -1)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
                string cs = _config.GetConnectionString("Default");
                CoffeeDetails = _coffeeData.GetCoffeeDetails(Id).Result.FirstOrDefault();

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
                int KyleTaste;
                int RhiannTaste;
                int KyleExperience;
                int RhiannExperience;
                if (Request.Form["UpdateCoffeeDetails.KyleTaste"].ToString() == "")
                {
                    KyleTaste = 0;
                } else
                {
                    KyleTaste = Convert.ToInt16(Request.Form["UpdateCoffeeDetails.KyleTaste"]);
                }
                if (Request.Form["UpdateCoffeeDetails.RhiannTaste"].ToString() == "")
                {
                    RhiannTaste = 0;
                } else
                {
                    RhiannTaste = Convert.ToInt16(Request.Form["UpdateCoffeeDetails.RhiannTaste"]);
                }
                if (Request.Form["UpdateCoffeeDetails.KyleExperience"].ToString() == "")
                {
                    KyleExperience = 0;
                } else
                {
                    KyleExperience = Convert.ToInt16(Request.Form["UpdateCoffeeDetails.KyleExperience"]);
                }
                if (Request.Form["UpdateCoffeeDetails.RhiannExperience"].ToString() == "")
                {
                    RhiannExperience = 0;
                } else
                {
                    RhiannExperience = Convert.ToInt16(Request.Form["UpdateCoffeeDetails.RhiannExperience"]);
                }
                await _coffeeData.UpdateCoffee(Convert.ToInt16(Request.Form["UpdateCoffeeDetails.CoffeeID"]),
                    Request.Form["UpdateCoffeeDetails.CoffeeName"].ToString(),
                    KyleTaste,
                    RhiannTaste,
                    KyleExperience,
                    RhiannExperience);
                return RedirectToPage("./View", new { @Id = Request.Form["UpdateCoffeeDetails.CoffeeID"].ToString(), @redirect = "update"  });
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
                await _coffeeData.ArchiveCoffee(UpdateCoffeeDetails.CoffeeID);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
