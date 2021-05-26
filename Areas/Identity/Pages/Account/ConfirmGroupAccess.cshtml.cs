using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatePot.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace DatePot.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmGroupAccessModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISiteData _siteData;

        public ConfirmGroupAccessModel(UserManager<IdentityUser> userManager,
        ISiteData siteData)
        {
            _userManager = userManager;
            _siteData = siteData;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string chosenUserId, string UserGroupID, string response)
        {
            // if (userId == null || chosenUserId == null || UserGroupID == null || response == null)
            // {
            //     return RedirectToPage("/Index");
            // }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            if (response.ToLower() == "true") {
                await _siteData.AcceptInvite(chosenUserId, Convert.ToInt32(UserGroupID));
            } else {
                await _siteData.RejectInvite(chosenUserId, Convert.ToInt32(UserGroupID), userId);
            }
            return Page();
        }
    }
}
