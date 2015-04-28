using System.Web.Http;
using asp.net_mvc.App_Start;

namespace asp.net_mvc.Controllers
{
    public class SerializationController : ApiController
    {
        public IHttpActionResult GetSerialization(int size)
        {
            var serialized = Json(StartUp.JSONData[size]);
            var deSerialized = serialized.ToString();
            
            return Ok(serialized);
        }
    }
}
