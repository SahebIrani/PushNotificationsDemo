using System.Collections.Generic;
using System.Linq;
using System.Net;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using SImple.SInjulMSBH.Models;

using WebPush;

namespace push_notification_angular_dotnet_core.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class NotificationController : Controller
	{
		public static List<PushSubscription> Subscriptions { get; set; } = new List<PushSubscription>();

		[HttpPost("subscribe")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public void Subscribe([FromBody] PushSubscription sub) => Subscriptions.Add(sub);

		[HttpPost("unsubscribe")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public void Unsubscribe([FromBody] PushSubscription sub)
		{
			PushSubscription item = Subscriptions.FirstOrDefault(s => s.Endpoint == sub.Endpoint);
			if (item != null)
				Subscriptions.Remove(item);
		}

		[HttpPost("broadcast")]
		[ProducesResponseType((int)HttpStatusCode.OK)]
		public void Broadcast([FromBody] NotificationModel message, [FromServices] VapidDetails vapidDetails)
		{
			WebPushClient client = new WebPushClient();
			string serializedMessage = JsonConvert.SerializeObject(message);
			foreach (var pushSubscription in Subscriptions)
				client.SendNotification(pushSubscription, serializedMessage, vapidDetails);
		}
	}
}
