using System;
using System.Collections.Generic;
using System.Web.Http;
using asp.net_mvc.App_Start;
using Jose;
using Security.Cryptography;

namespace asp.net_mvc.Controllers
{
    public class JWTController : ApiController
    {
        public IHttpActionResult GetJWT(string type)
        {
            var payload = new Dictionary<string, object>()
            {
                {"iss", "John Doe"}
            };

            switch (type)
            {
                case "HS256":
                    
                    string hstoken = JWT.Encode(payload, StartUp.SecretByteKey, JwsAlgorithm.HS256);
                    string hsjson = JWT.Decode(hstoken, StartUp.SecretByteKey);
                    return Ok();

                case "RS256":
                    

                    string rstoken = JWT.Encode(payload, StartUp.publicAndPrivate, JwsAlgorithm.RS256);
                    string rsjson = JWT.Decode(rstoken, StartUp.publicAndPrivate);
                    return Ok();

                case "ES256":

                    var privateKey = EccKey.New(StartUp.EccX, StartUp.EccY, StartUp.EccD);

                    string estoken = JWT.Encode(payload, privateKey, JwsAlgorithm.ES256);
                    string esjson = JWT.Decode(estoken, privateKey);
                    return Ok();

                case "none":
                    
                    string nantoken = JWT.Encode(payload, null, JwsAlgorithm.none);
                    string nanjson = JWT.Decode(nantoken, null);
                    return Ok();

                default:
                    return InternalServerError();
            }

        }
    }
    /*public class Authorization 
    { 
        public static byte[] GetBytes(string input) 
        { 
            var bytes = new byte[input.Length * sizeof(char)]; 
            Buffer.BlockCopy(input.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;   
        } 
    }*/
}
