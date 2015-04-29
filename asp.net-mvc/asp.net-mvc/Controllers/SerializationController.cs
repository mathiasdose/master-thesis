using System.Collections.Generic;
using System.Web.Http;
using asp.net_mvc.App_Start;
using asp.net_mvc.models;
using Newtonsoft.Json;

namespace asp.net_mvc.Controllers
{
    public class SerializationController : ApiController
    {
        public IHttpActionResult GetSerialization(int id)
        {
            var deSerialized = JsonConvert.DeserializeObject<List<SerialObject>>(StartUp.JSONData[id]);
            var serialized = JsonConvert.SerializeObject(deSerialized);
            
            return Ok();
        }
    }
}
