using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using static DatePot.Areas.VinylPot.Models.Vinyls;
using DatePot.Areas.VinylPot.Data;
using DatePot.Areas.FilmPot.Data;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Http;

namespace DatePot.Areas.VinylPot.Pages
{
	[ValidateAntiForgeryToken]
	public class ViewModel : PageModel
	{
		private readonly IConfiguration _config;
		private readonly IVinylData _VinylData;
		private readonly IFilmData _filmData;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		public ViewModel(IConfiguration config,
		IVinylData VinylData,
		IFilmData filmData,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData)
		{
			_config = config;
			_VinylData = VinylData;
			_filmData = filmData;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
		}
		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }
		[BindProperty]
		public UpdateVinylDetails UpdateVinylDetails { get; set; }
		public List<SelectListItem> Users { get; set; }
		public VinylDetails VinylDetails { get; set; }
		public List<PotAccess> PotAccess { get; set; }
		//public string Genre { get; set; }
		public async Task<IActionResult> OnGet()
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
				int index = PotAccess.FindIndex(item => item.PotID == 5);
				if (index == -1)
				{
					return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
				}
				VinylDetails = _VinylData.GetVinylDetails(Id).Result.FirstOrDefault();
				if (VinylDetails.UserGroupID != UserGroupID)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
				if (VinylDetails != null)
				{
					var users = _siteData.GetUserList(UserGroupID, 5);

					Users = new List<SelectListItem>();
					users.Result.ForEach(x =>
					{
						if (VinylDetails.AddedByID == x.UserID)
						{
							Users.Add(new SelectListItem { Value = x.UserID.ToString(), Text = x.UserName, Selected = true });
						}
						else
						{
							Users.Add(new SelectListItem { Value = x.UserID.ToString(), Text = x.UserName });
						}
					});
				}
				return Page();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPost(int VinylID, string Name, string ArtistName, bool Purchased, int AddedByID)
		{
			try
			{
				JsonResult result = null;
				string cs = _config.GetConnectionString("Default");
				await _VinylData.UpdateVinyl(VinylID, Name, ArtistName, Purchased, AddedByID);

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
