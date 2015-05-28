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
                var entity = new world()
                {
                    id = int.Parse(StartUp.Indexes.GetValue(randomId).ToString()),
                    randomInteger = StartUp.WorldObject.randomInteger,
                    randomString = StartUp.WorldObject.randomString,
                    randomDecimal = StartUp.WorldObject.randomDecimal,
                    randomDate = StartUp.WorldObject.randomDate
                }; 

                db.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                
                db.SaveChanges();

            }
            return Ok();
        }
    }
}
