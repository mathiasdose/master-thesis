using System;
using System.Configuration;
using System.Web.Http;
using asp.net_mvc.App_Start;

namespace asp.net_mvc.Controllers
{
    public class MyUpdateController : ApiController
    {
        public IHttpActionResult GetMyData()
        {
            var random = new Random();
            var randomId = random.Next(0, StartUp.Indexes.Length);

            using (var db = new MyDbContainer())
            {
                var entity = db.world.Find(StartUp.Indexes.GetValue(randomId));
                entity.randomNumber = random.Next(0, 10000);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                
                db.SaveChanges();

            }
            return Ok();
        }
    }
}
