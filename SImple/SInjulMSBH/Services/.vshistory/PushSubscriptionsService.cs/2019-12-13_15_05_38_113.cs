using System;
using System.Collections.Generic;

using Lib.Net.Http.WebPush;

using LiteDB;

namespace Demo.AspNetCore.Angular.PushNotifications.Services
{
	internal class PushSubscriptionsService : IPushSubscriptionsService, IDisposable
	{
		private class LitePushSubscription : PushSubscription
		{
			public LitePushSubscription() { }
			public LitePushSubscription(PushSubscription subscription)
			{
				Endpoint = subscription.Endpoint;
				Keys = subscription.Keys;
			}

			public int Id { get; set; }
		}

		public PushSubscriptionsService()
		{
			_db = new LiteDatabase("PushSubscriptionsStore.db");
			_collection = _db.GetCollection<LitePushSubscription>("subscriptions");
		}
		private readonly LiteDatabase _db;
		private readonly LiteCollection<LitePushSubscription> _collection;

		public IEnumerable<PushSubscription> GetAll() => _collection.FindAll();

		public void Insert(PushSubscription subscription) => _collection.Insert(new LitePushSubscription(subscription));

		public void Delete(BsonValue endpoint)
		{
			//_collection.Delete(subscription => subscription.Endpoint == endpoint);
			_collection.Delete(endpoint);
		}

		public void Dispose() => _db.Dispose();
	}
}
