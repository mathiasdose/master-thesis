using System;
using System.Configuration;
using System.Web.Http;
using asp.net_mvc.App_Start;

namespace asp.net_mvc.Controllers
{
    public class MyCreateController : ApiController
    {
        public IHttpActionResult GetMyData()
        {
            
            using (var db = new MyDbContainer())
            {
                db.world.Add(StartUp.WorldObject);
                db.SaveChanges();
            }

            return Ok();
        }

    }
}
