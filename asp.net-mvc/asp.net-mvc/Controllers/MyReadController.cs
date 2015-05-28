using System;
using System.Web.Http;
using System.Web.UI;
using asp.net_mvc.App_Start;

namespace asp.net_mvc.Controllers
{
    public class MyReadController : ApiController
    {
        public IHttpActionResult GetMyData()
        {
            var randomId = new Random().Next(0, StartUp.Indexes.Length);
            world entity;

            using (var db = new MyDbContainer())
            {    
                entity = db.world.Find(StartUp.Indexes.GetValue(randomId));
            }

            return Ok();
        }
    }
}
