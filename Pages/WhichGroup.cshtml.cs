using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using DatePot.Data;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Http;

namespace DatePot.Pages
{
	public class WhichGroupModel : PageModel
	{
		private readonly ILogger<WhichGroupModel> _logger;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		public WhichGroupModel(ILogger<WhichGroupModel> logger,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData)
		{
			_logger = logger;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
		}
		public List<PotAccess> PotAccess { get; set; }
		public List<UserGroups> UserGroups { get; set; }
		public async Task<ActionResult> OnGet()
		{
			if (!User.Identity.IsAuthenticated)
			{
				return RedirectToPage("/Account/Login", new { area = "Identity" });
			}
			try
			{
				var user = await _userManager.GetUserAsync(User);
				UserGroups = await _siteData.GetUserGroups(user.Id.ToString());
				var UsersName = _identityData.GetUser(user.Id.ToString()).Result.FirstOrDefault().Name.ToString();
				for (int i = 0; i < UserGroups.Count(); i++)
				{
					if (UserGroups[i].Name == UsersName.ToString())
					{
						var Id = UserGroups[i].UserGroupID;
						UserGroups.RemoveAt(i);
						UserGroups.Add(new UserGroups { UserGroupID = Id, Name = "Mine" });
					}
				}
				return Page();
			}
			catch (Exception er)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(ex.ToString());
			}
		}
		public async Task<ActionResult> OnPost(IFormCollection collection)
		{
			try
			{
				int i = Convert.ToInt32(collection["hidUserGroupID"]);
				HttpContext.Session.SetInt32("UserGroupID", i);
				return RedirectToPage("Index");
			}
			catch (Exception ex)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(ex.ToString());
			}
		}
	}
}
