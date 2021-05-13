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
using DatePot.Data;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using static DatePot.Models.Site;

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
                var UserID = _identityData.GetUser(user.Id.ToString()).Result.FirstOrDefault().UserID.ToString();
                PotAccess = await _siteData.GetPotAccess(Convert.ToInt32(UserID));
                return Page();
            }
        }
    }
}
