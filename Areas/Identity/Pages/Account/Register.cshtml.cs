using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using DatePot.Areas.Identity.Data;
using WebPWrecover.Services;

namespace DatePot.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;
        private readonly IIdentityData _identityData;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IConfiguration config,
            IIdentityData identityData)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _config = config;
            _identityData = identityData;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    string cs = _config.GetConnectionString("Default");
                    _identityData.AddUser(Input.Name.ToString(), user.Id);
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    string sBody = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                        <html xmlns='http://www.w3.org/1999/xhtml' lang='en-GB'>
                        <head>
                        <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
                        <title>The Date Pot</title>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'/>

                        <style type='text/css'>
                            a[x-apple-data-detectors] {color: inherit !important;}
                        </style>

                        </head>
                        <body style='margin: 0; padding: 0;'>
                        <table role='presentation' border='0' cellpadding='0' cellspacing='0' width='100%'>
                            <tr>
                            <td style='padding: 20px 0 30px 0;'>

                        <table align='center' border='0' cellpadding='0' cellspacing='0' width='600' style='border-collapse: collapse; border: 1px solid #cccccc;'>
                        <tr>
                            <td align='center' bgcolor='#70bbd9'>
                            <img src='https://thedatepot.co.uk/main/images/emailImages/EMAIL_HEADER.svg' alt='The Date Pot. Everything is my life needs it own website.' width='600' height='320' style='display: block;' />
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor='#ffffff' style='padding: 40px 30px 40px 30px;'>
                            <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>
                                <tr>
                                <td style='color: #153643; font-family: Arial, sans-serif;'>
                                    <h1 style='font-size: 24px; margin: 0;'>Cheers for registering</h1>
                                </td>
                                </tr>
                                <tr>
                                <td style='color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 24px; padding: 20px 0 30px 0;'>
                                    <p style='margin: 0;'>Please confirm your account by <a href='" + HtmlEncoder.Default.Encode(callbackUrl) + @"'>clicking here</a>.</p>
                                    <p style='margin: 0;'>The Date Pot is a little project I slowly (sometimes very slowly) add little bits and pieces as and when management (<a href='https://www.instagram.com/rhiann_bull/'>@rhiannbull</a>) decides a new feature is needed.</p>
                                    <p style='margin: 0;'>If you think of something you would like added you can email me @ scud1997@gmail.com or add it as an <a href='https://github.com/kylescudder/DatePot/issues'>issue</a> here if you are familiar with GitHub.</p>
                                </td>
                                </tr>
                                <tr>
                                <td>
                                    <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>
                                    <tr>
                                        <td width='260' valign='top'>
                                        <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>
                                            <tr>
                                            <td>
                                                <img src='https://thedatepot.co.uk/main/images/emailImages/EMAIL_BOXES-01.svg' alt='' width='100%' height='140' style='display: block;' />
                                            </td>
                                            </tr>
                                            <tr>
                                            <td style='color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 24px; padding: 25px 0 0 0;'>
                                                <p style='margin: 0;'>If you have any questions feel free to get in touch. I hope you and yours find it as fun as me and Rhiann have. 😁</p>
                                            </td>
                                            </tr>
                                        </table>
                                        </td>
                                        <td style='font-size: 0; line-height: 0;' width='20'>&nbsp;</td>
                                        <td width='260' valign='top'>
                                        <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>
                                            <tr>
                                            <td>
                                                <img src='https://thedatepot.co.uk/main/images/emailImages/EMAIL_BOXES-02.svg' alt='' width='100%' height='140' style='display: block;' />
                                            </td>
                                            </tr>
                                            <tr>
                                            <td style='color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 24px; padding: 25px 0 0 0;'>
                                                <p style='margin: 0;'>Your emails are stored but only for the purpose of a username, I will never do anything with it other than store it for the express purpose of using it as a username. If you ever want to delete your account and all data stored about you, please head to the account page and use the Delete button. This can be found <a href='https://thedatepot.co.uk/main/Identity/Account/Manage/PersonalData'>here</a>.</p>
                                            </td>
                                            </tr>
                                        </table>
                                        </td>
                                    </tr>
                                    </table>
                                </td>
                                </tr>
                            </table>
                            </td>
                        </tr>
                        <tr>
                            <td bgcolor='#ee4c50' style='padding: 30px 30px;'>
                                <table border='0' cellpadding='0' cellspacing='0' width='100%' style='border-collapse: collapse;'>
                                <tr>
                                <td style='color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;'>
                                    <p style='margin: 0;'>&#169; Kyle Scudder 2021 - maybe?<br/>
                                </td>
                                <td align='right'>
                                    <table border='0' cellpadding='0' cellspacing='0' style='border-collapse: collapse;'>
                                    <tr>
                                        <td>
                                        <a href='https://www.instagram.com/scudderkyle/'>
                                            <img src='https://thedatepot.co.uk/main/images/emailImages/LOGOS-01.svg' alt='Twitter.' width='38' height='38' style='display: block;' border='0' />
                                        </a>
                                        </td>
                                        <td style='font-size: 0; line-height: 0;' width='20'>&nbsp;</td>
                                        <td>
                                        <a href='https://github.com/kylescudder'>
                                            <img src='https://thedatepot.co.uk/main/images/emailImages/LOGOS-02.svg' alt='Facebook.' width='38' height='38' style='display: block;' border='0' />
                                        </a>
                                        </td>
                                    </tr>
                                    </table>
                                </td>
                                </tr>
                            </table>
                            </td>
                        </tr>
                        </table>

                            </td>
                            </tr>
                        </table>
                        </body>
                        </html>";
                    await _emailSender.SendEmailAsync(Input.Email, "DatePot - Please confirm your email 📧", sBody);

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
