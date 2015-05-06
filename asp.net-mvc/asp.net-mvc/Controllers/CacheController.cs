using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Http;
using StackExchange.Redis;

namespace asp.net_mvc.Controllers
{
    public class CacheController : ApiController
    {
        public IHttpActionResult GetCache(int id)
        {
            List<string> value = new List<string>();
            using (var redis = ConnectionMultiplexer.Connect("localhost"))
            {
                IDatabase cacheDatabase = redis.GetDatabase();
                for (int i = 0; i < Math.Floor((decimal)id/2); i++)
                {
                    value.Add(cacheDatabase.HashGet(cacheDatabase.KeyRandom(), "field1"));
                    value.Add(cacheDatabase.HashGet(cacheDatabase.KeyRandom(), "field2"));
                }
            }
            return Ok(new {value});
        }
    }
}
