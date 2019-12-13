using Demo.AspNetCore.Angular.PushNotifications.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Demo.AspNetCore.Angular.PushNotifications.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PublicKeyController : ControllerBase
	{
		public PublicKeyController(IOptions<PushNotificationsOptions> options) => _options = options.Value;

		private readonly PushNotificationsOptions _options;

		public ContentResult Get() => Content(_options.PublicKey, "text/plain");
	}
}