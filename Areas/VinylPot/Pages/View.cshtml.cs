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
using static DatePot.Areas.VinylPot.Models.Vinyls;
using DatePot.Areas.VinylPot.Data;

namespace DatePot.Areas.VinylPot.Pages
{
    [ValidateAntiForgeryToken]
    public class ViewModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly IVinylData _VinylData;
        public ViewModel(IConfiguration config, IVinylData VinylData)
        {
            _config = config;
            _VinylData = VinylData;
        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public UpdateVinylDetails UpdateVinylDetails { get; set; }

        public VinylDetails VinylDetails { get; set; }
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
                VinylDetails = _VinylData.GetVinylDetails(Id).Result.FirstOrDefault();

                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<JsonResult> OnPost(int VinylID, string Name, string ArtistName, bool Purchased)
        {
            try
            {
                JsonResult result = null;
                string cs = _config.GetConnectionString("Default");
                await _VinylData.UpdateVinyl(VinylID, Name, ArtistName, Purchased);

                result = new JsonResult(VinylID);
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
                await _VinylData.ArchiveVinyl(UpdateVinylDetails.VinylID);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
