using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using static DatePot.Areas.HolidayPot.Models.Holidays;
using DatePot.Areas.HolidayPot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using Microsoft.AspNetCore.Http;
using DatePot.Areas.FilmPot.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DatePot.Areas.HolidayPot.Pages
{
    [ValidateAntiForgeryToken]
    public class ViewModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly IHolidayData _HolidayData;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityData _identityData;
        private readonly ISiteData _siteData;
        private readonly IFilmData _filmData;
        private IHostingEnvironment _environment;
        public ViewModel(IConfiguration config,
        IHolidayData HolidayData,
        UserManager<IdentityUser> userManager,
        IIdentityData identityData,
        ISiteData siteData,
        IFilmData filmData,
        IHostingEnvironment environment)
        {
            _config = config;
            _HolidayData = HolidayData;
            _userManager = userManager;
            _identityData = identityData;
            _siteData = siteData;
            _filmData = filmData;
            _environment = environment;
        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public UpdateHolidayDetails UpdateHolidayDetails { get; set; }
        [BindProperty]
        public List<FileModel> FileNameList { get; set; }
        public List<string> FileNames { get; set; }
        public HolidayDetails HolidayDetails { get; set; }
        public List<PotAccess> PotAccess { get; set; }
        public List<SelectListItem> Users { get; set; }
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
                int index = PotAccess.FindIndex(item => item.PotID == 7);
                if (index == -1)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
                string cs = _config.GetConnectionString("Default");
                HolidayDetails = _HolidayData.GetHolidayDetails(Id).Result.FirstOrDefault();               
                //Fetch all files in the Folder (Directory).
                string holidayPath = "images\\Holiday\\" + Id;
                var path = Path.Combine(_environment.WebRootPath, holidayPath);
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                string[] filePaths = Directory.GetFiles(path);

                //Copy File names to Model collection.
                FileNameList = new List<FileModel>();
                foreach (string filePath in filePaths)
                {
                    FileNameList.Add(new FileModel { FileName = Id + "/" + Path.GetFileName(filePath) });
                }
                if (HolidayDetails.UserGroupID != UserGroupID)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                await _HolidayData.UpdateHoliday(Convert.ToInt16(Request.Form["UpdateHolidayDetails.HolidayID"]),
                    Request.Form["UpdateHolidayDetails.Country"].ToString(),
                    Request.Form["UpdateHolidayDetails.City"].ToString(),
                    Convert.ToBoolean(Request.Form["UpdateHolidayDetails.Been"].ToString()),
                    Convert.ToDateTime(Request.Form["UpdateHolidayDetails.DateBeen"]));
                return RedirectToPage("./View", new { @Id = Request.Form["UpdateHolidayDetails.HolidayID"].ToString(), @redirect = "update" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<IActionResult> OnPostFileUploadAsync(IFormFile[] photos)
        {
            try
            {

                string Id = HttpContext.Request.Form["HolidayID"].ToString();
                if (photos != null && photos.Length > 0)
                {
                    FileNames = new List<string>();
                    foreach (IFormFile photo in photos)
                    {
                        string holidayPath = "images\\Holiday\\" + Id;
                        var path = Path.Combine(_environment.WebRootPath, holidayPath, photo.FileName);
                        var stream = new FileStream(path, FileMode.Create);
                        photo.CopyToAsync(stream);
                        FileNames.Add(photo.FileName);
                    }
                }
                return RedirectToPage("./View", new { @Id = Id, @redirect = "update" });
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
                await _HolidayData.ArchiveHoliday(UpdateHolidayDetails.HolidayID);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
