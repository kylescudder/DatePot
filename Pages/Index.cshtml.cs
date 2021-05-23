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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityData _identityData;
        private readonly ISiteData _siteData;
        public IndexModel(ILogger<IndexModel> logger,
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
        public int? CurrentUserGroupID { get; set; }
        public int? DefaultUserGroupID { get; set; }
        public async Task<ActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else {
                var user = await _userManager.GetUserAsync(User);
                DefaultUserGroupID = await _siteData.GetDefaultUserGroupID(user.Id.ToString());
                if (DefaultUserGroupID == 0) {
                    if (HttpContext.Session.GetInt32("UserGroupID") == null) {
                        UserGroups = await _siteData.GetUserGroups(user.Id.ToString());
                        int OwnUserGroupID = await _siteData.GetUserOwnGroup(user.Id.ToString());
                        for (int i = 0; i < UserGroups.Count(); i++)
                        {
                            if (OwnUserGroupID != UserGroups[i].UserGroupID) {
                                return RedirectToPage("WhichGroup");
                            } else {
                                DefaultUserGroupID = OwnUserGroupID;
                                PotAccess = await _siteData.GetPotAccess(user.Id.ToString(), DefaultUserGroupID);
                            }
                        }
                    } else {
                        CurrentUserGroupID = HttpContext.Session.GetInt32("UserGroupID");
                        DefaultUserGroupID = CurrentUserGroupID;
                        PotAccess = await _siteData.GetPotAccess(user.Id.ToString(), CurrentUserGroupID);
                        return Page();
                    }
                }
                PotAccess = await _siteData.GetPotAccess(user.Id.ToString(), DefaultUserGroupID);
                HttpContext.Session.SetInt32("UserGroupID", DefaultUserGroupID.Value);
                return Page();
            }
        }
        public async Task<JsonResult> OnPost(IFormCollection collection)
        {
            try {
                JsonResult result = null;
                int UserGroupID = Convert.ToInt32(collection["hidUserGroupID"]);
                var user = await _userManager.GetUserAsync(User);
                await _siteData.SetDefaultUserGroupID(UserGroupID, user.Id.ToString());
                result = new JsonResult("success");
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
        }
    }
}
