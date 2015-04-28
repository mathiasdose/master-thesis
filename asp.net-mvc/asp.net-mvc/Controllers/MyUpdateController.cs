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
            world old = null;
            world updated = null;

            using (var db = new MyDbContainer())
            {
                var randomId = random.Next(0, StartUp.Indexes.Length);

                var entity = db.world.Find(StartUp.Indexes.GetValue(randomId));
                if (bool.Parse(ConfigurationManager.AppSettings["TestMode"]))
                    old = new world()
                    {
                        id = entity.id,
                        randomNumber = entity.randomNumber
                    };
                
                entity.randomNumber = random.Next(0, 10000);
                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                
                db.SaveChanges();

                if (bool.Parse(ConfigurationManager.AppSettings["TestMode"]))
                    updated = db.world.Find(StartUp.Indexes.GetValue(randomId));
            }
            if (bool.Parse(ConfigurationManager.AppSettings["TestMode"]))
                return Ok(new {old, updated});
            return Ok();
        }
    }
}
