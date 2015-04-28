using System;
using System.Web.Http;

namespace asp.net_mvc.Controllers
{
    public class MyCreateController : ApiController
    {
        Random random = new Random();

        public IHttpActionResult GetMyData()
        {   
            using (var db = new MyDbContainer())
            {
                db.world.Add(new world()
                {
                    randomNumber = random.Next(0, 10000)
                });
                db.SaveChanges();
            }

            return Ok();
        }
    }
}
