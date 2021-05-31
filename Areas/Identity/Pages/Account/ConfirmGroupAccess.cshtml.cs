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
            if (await _siteData.IsLinkConsumed("userID=" + userId + "&amp;chosenUserId="
                + chosenUserId + "&amp;UserGroupID=" + UserGroupID + "&amp;")) {
                ViewData["ResponseText"] = "You have already replied to this invite.";
                ViewData["Title"] = "You have already responded!";
                return Page();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewData["ResponseText"] = $"Unable to load user with ID '{userId}'.";
                ViewData["Title"] = "Something went screwy. 😕";
                return Page();
            }
            if (response.ToLower() == "true") {
                await _siteData.AcceptInvite(chosenUserId, Convert.ToInt32(UserGroupID));
                ViewData["ResponseText"] = "Thanks for accepting the invite! Now you can share all the best things with the people you hold dearest! ☮";
                ViewData["Title"] = "Accept invite";
                return Page();
            }
            await _siteData.RejectInvite(chosenUserId, Convert.ToInt32(UserGroupID), userId);
            ViewData["ResponseText"] = "Sad times my friend, sorry to see you reject it, but respect your choice. 🤘";
            ViewData["Title"] = "Reject invite";

            await _siteData.ConsumeLink("userID=" + userId + "&amp;chosenUserId="
                + chosenUserId + "&amp;UserGroupID=" + UserGroupID + "&amp;");
            return Page();
        }
    }
}
