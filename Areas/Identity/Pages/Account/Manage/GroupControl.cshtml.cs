using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DatePot.Areas.Identity.Data;
using Microsoft.Extensions.Configuration;
using static DatePot.Models.Site;
using DatePot.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using static DatePot.Areas.Identity.Models.Identity;

namespace DatePot.Areas.Identity.Pages.Account.Manage
{
	public partial class GroupControlModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IIdentityData _identityData;
        private readonly ISiteData _siteData;

        public GroupControlModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration config,
            IIdentityData identityData,
            ISiteData siteData)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _identityData = identityData;
            _siteData = siteData;
        }
        [TempData]
        public string StatusMessage { get; set; }
		public List<SelectListItem> UserAccessToGroup { get; set; }
        public List<PotAccess> PotAccess { get; set; }

        private async Task LoadAsync(IdentityUser user)
        {
            var useraccesstogroup = _siteData.GetUserAccessToGroup(user.Id.ToString());
            UserAccessToGroup = new List<SelectListItem>();

            useraccesstogroup.Result.ForEach(x =>
            {
                UserAccessToGroup.Add(new SelectListItem { Value = x.UserName.ToString(), Text = x.UserName });
            });
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<PartialViewResult> OnPostAsync(string UserID)
        {
            try {
                JsonResult result = null;
                var user = await _userManager.GetUserAsync(User);
                var ChosenUser = await _userManager.FindByEmailAsync(UserID);
                int UserOwnGroupID = await _siteData.GetUserOwnGroup(user.Id.ToString());
                PotAccess = await _siteData.GetPotAccess(ChosenUser.Id.ToString(), UserOwnGroupID);
				return new PartialViewResult
				{
					ViewName = "_UserAccessToGroup",
					ViewData = new ViewDataDictionary<List<UserAccessToGroup>>(ViewData, PotAccess)
				};                
                // result = new JsonResult("success");
				// return result;
            }
            catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
        }
    }
}
