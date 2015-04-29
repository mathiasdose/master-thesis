using System;
using System.Configuration;
using System.Web.Http;

namespace asp.net_mvc.Controllers
{
    public class MyCreateController : ApiController
    {
        public IHttpActionResult GetMyData()
        {
            var world = new world()
                {
                    randomNumber = new Random().Next(0, 10000)
                };
            using (var db = new MyDbContainer())
            {
                db.world.Add(world);
                db.SaveChanges();
            }

            return Ok();
        }

    }
}
