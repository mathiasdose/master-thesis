using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Http;
using asp.net_mvc.App_Start;
using StackExchange.Redis;

namespace asp.net_mvc.Controllers
{
    public class CacheController : ApiController
    {
        public IHttpActionResult GetCache(int id)
        {
            var randomId = new Random().Next(0, StartUp.CacheKeys.Length);

            using (var redis = ConnectionMultiplexer.Connect("localhost"))
            {
                IDatabase cacheDatabase = redis.GetDatabase();
                for (int i = 0; i < Math.Floor((decimal)id/2); i++)
                {
                    var value = cacheDatabase.HashGetAll(StartUp.CacheKeys.GetValue(randomId).ToString());
                }
            }
            return Ok();
        }
    }
}
