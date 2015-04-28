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
            Random random = new Random();
            world entity;
            using (var db = new MyDbContainer())
            {
                db.Database.CreateIfNotExists();
     
                var randomID = random.Next(0, StartUp.Indexes.Length);
                entity = db.world.Find(StartUp.Indexes.GetValue(randomID));
            }

            return Ok();
        }
    }
}
