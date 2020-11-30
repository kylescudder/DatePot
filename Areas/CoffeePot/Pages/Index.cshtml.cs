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
using static DatePot.Areas.CoffeePot.Models.Coffees;
using DatePot.Areas.CoffeePot.Data;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DatePot.Areas.CoffeePot.Pages
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
        CoffeeData fd = new CoffeeData();
        public List<CoffeeList> Coffees { get; set; }
        public NewCoffee NewCoffee { get; set; }
        public List<RandomCoffee> RandomCoffees { get; set; }
        public ActionResult OnGet()
         {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
                string cs = _config.GetConnectionString("Default");
                Coffees = fd.GetCoffeeList(cs);

                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<JsonResult> OnPost(string CoffeeName)
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                int CoffeeID = fd.AddCoffee(cs, CoffeeName);

                JsonResult result = null;
                result = new JsonResult(CoffeeID);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
