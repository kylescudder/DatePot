using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebPWrecover.Services
{
	public class EmailSender : IEmailSender
	{
		private readonly IConfiguration _config;
		public EmailSender(IConfiguration config)
		{
			_config = config;
		}
		public Task SendEmailAsync(string email, string subject, string message)
		{
			return Execute(subject, message, email);
		}
		public Task Execute(string subject, string message, string email)
		{
			MailMessage m = new MailMessage();
			m.From = new MailAddress(_config.GetSection("SMTP")["From"]);
			m.To.Add(new MailAddress(email));
			m.Subject = subject;
			m.IsBodyHtml = true;
			m.Body = message;

			SmtpClient c = new SmtpClient();
			c.Host = _config.GetSection("SMTP")["Host"];
			c.Port = Convert.ToInt16(_config.GetSection("SMTP")["Port"]);
			c.Credentials = new System.Net.NetworkCredential(_config.GetSection("SMTP")["Username"], _config.GetSection("SMTP")["Password"]);
			return c.SendMailAsync(m);
		}
	}
}