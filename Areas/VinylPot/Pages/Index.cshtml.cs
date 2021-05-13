using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static DatePot.Areas.VinylPot.Models.Vinyls;
using DatePot.Areas.VinylPot.Data;
using DatePot.Areas.FilmPot.Data;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using System.Linq;
using static DatePot.Models.Site;

namespace DatePot.Areas.VinylPot.Pages
{
    [ValidateAntiForgeryToken]
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IConfiguration _config;
		private readonly IVinylData _VinylData;
		private readonly IFilmData _filmData;
		private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityData _identityData;
        private readonly ISiteData _siteData;
		public IndexModel(ILogger<IndexModel> logger, 
		IConfiguration config, 
		IVinylData VinylData, 
		IFilmData filmData,
		UserManager<IdentityUser> userManager,
        IIdentityData identityData,
        ISiteData siteData)
		{
			_logger = logger;
			_config = config;
			_VinylData = VinylData;
			_filmData = filmData;
            _userManager = userManager;
            _identityData = identityData;
            _siteData = siteData;

		}
		public List<VinylList> Vinyls { get; set; }
		public List<SelectListItem> Users { get; set; }
		public NewVinyl NewVinyl { get; set; }
		public List<PotAccess> PotAccess { get; set; }
		public async Task<ActionResult> OnGet()
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
				int index = PotAccess.FindIndex(item => item.PotID == 5);
                if (index == -1)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
                Vinyls = _VinylData.GetVinylList().Result;
                var users = _filmData.GetUserList();

                Users = new List<SelectListItem>();
                users.Result.ForEach(x =>
                {
                    Users.Add(new SelectListItem { Value = x.UserID.ToString(), Text = x.UserName });
                });
                return Page();
            }
            catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
        public async Task<JsonResult> OnPost(string VinylName, string VinylArtistName, bool VinylPurchased, int VinylAddedByID)
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
				int VinylID = _VinylData.AddVinyl(VinylName, VinylArtistName, VinylPurchased, VinylAddedByID).Result;

				result = new JsonResult(VinylID);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
	}
}
