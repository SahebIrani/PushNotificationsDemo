﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Demo.AspNetCore.Angular.PushNotifications.Services;

namespace Demo.AspNetCore.Angular.PushNotifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicKeyController : ControllerBase
    {
        private readonly PushNotificationsOptions _options;

        public PublicKeyController(IOptions<PushNotificationsOptions> options)
        {
            _options = options.Value;
        }

        public ContentResult Get()
        {
            return Content(_options.PublicKey, "text/plain");
        }
    }
}