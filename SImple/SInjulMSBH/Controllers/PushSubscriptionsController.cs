using System.Net;

using Demo.AspNetCore.Angular.PushNotifications.Services;

using Lib.Net.Http.WebPush;

using Microsoft.AspNetCore.Mvc;

namespace Demo.AspNetCore.Angular.PushNotifications.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PushSubscriptionsController : ControllerBase
	{
		public PushSubscriptionsController(IPushSubscriptionsService pushSubscriptionsService) =>

			_pushSubscriptionsService = pushSubscriptionsService;
		private readonly IPushSubscriptionsService _pushSubscriptionsService;

		[HttpPost]
		public void Post([FromBody] PushSubscription subscription) => _pushSubscriptionsService.Insert(subscription);

		[HttpDelete("{endpoint}")]
		public void Delete(string endpoint) => _pushSubscriptionsService.Delete(WebUtility.UrlDecode(endpoint));
	}
}
