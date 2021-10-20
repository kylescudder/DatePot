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
using Microsoft.AspNetCore.Http;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace DatePot.Areas.Identity.Pages.Account.Manage
{
	public partial class GroupControlModel : PageModel
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IConfiguration _config;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		private readonly IEmailSender _emailSender;

		public GroupControlModel(
				UserManager<IdentityUser> userManager,
				SignInManager<IdentityUser> signInManager,
				IConfiguration config,
				IIdentityData identityData,
				ISiteData siteData,
				IEmailSender emailSender)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_config = config;
			_identityData = identityData;
			_siteData = siteData;
			_emailSender = emailSender;
		}
		[TempData]
		public string StatusMessage { get; set; }
		public List<SelectListItem> UserAccessToGroup { get; set; }
		public List<Models.Identity.UserAccessToGroup> UserAccessToGroupList { get; set; }
		[BindProperty]
		public NewUserAccess NewUserAccess { get; set; }
		private async Task LoadAsync(IdentityUser user)
		{
			try
			{
				int UserOwnGroupID = await _siteData.GetUserOwnGroup(user.Id.ToString());
				HttpContext.Session.SetInt32("UserOwnGroupID", UserOwnGroupID);
				var useraccesstogroup = _siteData.GetUserAccessToGroup(user.Id.ToString(), UserOwnGroupID);
				UserAccessToGroup = new List<SelectListItem>();

				useraccesstogroup.Result.ForEach(x =>
				{
					UserAccessToGroup.Add(new SelectListItem { Value = x.UserName.ToString(), Text = x.UserName });
				});
			}
			catch (Exception er)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(er.ToString());
			}
		}

		public async Task<IActionResult> OnGetAsync()
		{
			try
			{
				var user = await _userManager.GetUserAsync(User);
				if (user == null)
				{
					return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
				}

				await LoadAsync(user);
				return Page();
			}
			catch (Exception er)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(er.ToString());
			}
		}

		public async Task<PartialViewResult> OnGetUserAccessToGroup(string UserID)
		{
			try
			{
				var user = await _userManager.GetUserAsync(User);
				var ChosenUser = await _userManager.FindByEmailAsync(UserID);
				int? UserOwnGroupID = HttpContext.Session.GetInt32("UserOwnGroupID");
				UserAccessToGroupList = await _siteData.GetUserPotAccess(ChosenUser.Id.ToString(), UserOwnGroupID);
				HttpContext.Session.SetInt32("UserAccessToGroupList", UserAccessToGroupList.Count());
				return new PartialViewResult
				{
					ViewName = "_UserAccessToGroup",
					ViewData = new ViewDataDictionary<List<Models.Identity.UserAccessToGroup>>(ViewData, UserAccessToGroupList)
				};
				// result = new JsonResult("success");
				// return result;
			}
			catch (Exception ex)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(ex.ToString());
			}
		}
		public async Task<IActionResult> OnPostUserGroupUpdated(IFormCollection collection)
		{
			try
			{
				JsonResult result = null;
				int? PotCount = HttpContext.Session.GetInt32("UserAccessToGroupList");
				string UserID = Request.Form["UserID"];
				List<DatePot.Models.Site.UserAccessToGroup> uatg = new List<DatePot.Models.Site.UserAccessToGroup>();
				foreach (var item in Request.Form.Keys)
				{
					if (item != "UserID" && item != "__RequestVerificationToken")
					{
						uatg.Add(new DatePot.Models.Site.UserAccessToGroup()
						{
							UserID = UserID,
							PotID = Convert.ToInt32(item),
							UserGroupID = HttpContext.Session.GetInt32("UserOwnGroupID").Value
						});
					}
				}
				await _siteData.UpdateUserAccessToGroup(uatg, UserID, HttpContext.Session.GetInt32("UserOwnGroupID").Value);
				result = new JsonResult("success");
				return result;
			}
			catch (Exception ex)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(ex.ToString());
			}
		}
		public async Task<IActionResult> OnPostAddUserAccess(IFormCollection collection)
		{
			try
			{
				JsonResult result = null;
				if (ModelState.IsValid)
				{
					var ChosenUser = await _userManager.FindByEmailAsync(Request.Form["NewUserAccess.UserEmail"][0]);
					if (ChosenUser != null)
					{
						var user = await _userManager.GetUserAsync(User);
						int PotCount = await _siteData.GetPotCount();
						List<RejectAudit> ra = await _siteData.GetRejectAudit(user.Id, ChosenUser.Id);
						if (ra.Count > 2)
						{
							result = new JsonResult("takeahint");
							return result;
						}
						else if (ra.Count != 0)
						{
							TimeSpan diff = DateTime.Now - ra.Last().RejectedDate;
							if (diff.TotalHours > 24)
							{
								result = await UpdateUserAccessToGroup(ChosenUser, PotCount, user, result);
								return result;
							}
							else
							{
								result = new JsonResult("tooquick");
								return result;
							}
						}
						else
						{
							result = await UpdateUserAccessToGroup(ChosenUser, PotCount, user, result);
							return result;
						}
					}
					else
					{
						result = new JsonResult("emailnotfound");
						return result;
					}
				}
				result = new JsonResult("emailnotvalid");
				return result;
			}
			catch (Exception ex)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(ex.ToString());
			}
		}
		public async Task<IActionResult> emailUser(IdentityUser ChosenUser, IdentityUser user, int UserGroupID)
		{
			try
			{

				var callbackUrl = Url.Page(
						"/Account/ConfirmGroupAccess",
						pageHandler: null,
						values: new
						{
							area = "Identity",
							userId = user.Id,
							chosenUserId = ChosenUser.Id,
							UserGroupID = UserGroupID,
							response = true
						},
						protocol: Request.Scheme);
				var callbackUrlReject = Url.Page(
						"/Account/ConfirmGroupAccess",
						pageHandler: null,
						values: new
						{
							area = "Identity",
							userId = user.Id,
							chosenUserId = ChosenUser.Id,
							UserGroupID = UserGroupID,
							response = false
						},
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
                            <h1 style='font-size: 24px; margin: 0;'>" + user.UserName + @" wants to share a little part of their life with you!</h1>
                            <p>Psst, take a minute to tell " + user.UserName + @" how much you appreciate it! 😊</p>
                        </td>
                        </tr>
                        <tr>
                        <td style='color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 24px; padding: 20px 0 30px 0;'>
                            <p style='margin: 0;'>To accept their invite <a href='" + HtmlEncoder.Default.Encode(callbackUrl) + @"'>click here</a>.</p>
                            <p style='margin: 0;'>To decline their invite <a href='" + HtmlEncoder.Default.Encode(callbackUrlReject) + @"'>click here</a>.</p>
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
				string[] callbackUrlArray = HtmlEncoder.Default.Encode(callbackUrl).Split("?");
				string[] InviteLinkArray = callbackUrlArray[1].Split("response");
				string InviteLink = InviteLinkArray[0];
				await _siteData.AddInviteLink(InviteLink, false);
				var client = new SmtpClient(_config.GetSection("SMTP")["Host"], Convert.ToInt16(_config.GetSection("SMTP")["Port"]))
				{
					Credentials = new NetworkCredential(_config.GetSection("SMTP")["Username"], _config.GetSection("SMTP")["Password"]),
					EnableSsl = true
				};
				MailMessage m = new MailMessage();
				m.From = new MailAddress(_config.GetSection("SMTP")["InviteFrom"]);
				m.To.Add(new MailAddress(ChosenUser.NormalizedEmail));
				m.Subject = "DatePot - You've been invited! 📧";
				m.IsBodyHtml = true;
				m.Body = sBody;
				client.Send(m);
				return Page();
			}
			catch (Exception er)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(er.ToString());
			}
		}
		public async Task<JsonResult> UpdateUserAccessToGroup(IdentityUser ChosenUser, int PotCount, IdentityUser user, JsonResult result)
		{
			try
			{
				int existingCount = await _siteData.GetExistingPotAccess(ChosenUser.Id, HttpContext.Session.GetInt32("UserOwnGroupID").Value);
				if (existingCount == 0)
				{
					emailUser(ChosenUser, user, HttpContext.Session.GetInt32("UserOwnGroupID").Value);
					List<DatePot.Models.Site.UserAccessToGroup> uatg = new List<DatePot.Models.Site.UserAccessToGroup>();
					for (int i = 0; i < PotCount; i++)
					{
						uatg.Add(new DatePot.Models.Site.UserAccessToGroup
						{
							UserID = ChosenUser.Id,
							PotID = i + 1,
							UserGroupID = HttpContext.Session.GetInt32("UserOwnGroupID").Value
						});
					}
					await _siteData.UpdateUserAccessToGroup(uatg, ChosenUser.Id.ToString(), HttpContext.Session.GetInt32("UserOwnGroupID").Value);
					result = new JsonResult("success");
					return result;
				}
				else
				{
					result = new JsonResult("useralreadyhasaccess");
					return result;
				}
			}
			catch (Exception er)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(er.ToString());
			}
		}
	}
}
