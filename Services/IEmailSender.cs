using System.Threading.Tasks;

namespace WebPWrecover.Services
{
	public interface IEmailSender
	{
		Task Execute(string subject, string message, string email);
		Task SendEmailAsync(string email, string subject, string message);
	}
}