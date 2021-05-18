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

namespace DatePot.Areas.Identity.Pages.Account.Manage
{
	public partial class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IIdentityData _identityData;
        private readonly ISiteData _siteData;

        public IndexModel(
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

        public string Username { get; set; }
        public string UserName { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
		public List<SelectListItem> UserGroups { get; set; }
        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Name")]
            public string UserName { get; set; }
            [Display(Name = "Default User Group")]
            public int DefaultUserGroupID { get; set; }
        }

        private async Task LoadAsync(IdentityUser user)
        {
            string cs = _config.GetConnectionString("Default");
            var userName = await _userManager.GetUserNameAsync(user);
            Identity.Models.Identity.UserList ul = new Identity.Models.Identity.UserList();
            ul = _identityData.GetUser(user.Id.ToString()).Result.FirstOrDefault();
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            
            var usergrouplist = _siteData.GetUserGroups(user.Id.ToString());
            UserGroups = new List<SelectListItem>();

            usergrouplist.Result.ForEach(x =>
            {
                if (ul.DefaultUserGroupID == x.UserGroupID)
                {
                    UserGroups.Add(new SelectListItem { Value = x.UserGroupID.ToString(), Text = x.Name, Selected = true });
                }
                else
                {
                    UserGroups.Add(new SelectListItem { Value = x.UserGroupID.ToString(), Text = x.Name });
                }
            });

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                DefaultUserGroupID = ul.DefaultUserGroupID,
                UserName = ul.Name
            };
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

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            await _siteData.SetDefaultUserGroupID(Input.DefaultUserGroupID, user.Id.ToString());
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
