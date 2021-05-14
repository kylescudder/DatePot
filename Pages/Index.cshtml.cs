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
        public async Task<ActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else {
                var user = await _userManager.GetUserAsync(User);
                PotAccess = await _siteData.GetPotAccess(user.Id.ToString());
                int UserGroupID = await _siteData.GetUserGroupID(user.Id.ToString());
                HttpContext.Session.SetInt32("UserGroupID", UserGroupID);
                return Page();
            }
        }
    }
}
