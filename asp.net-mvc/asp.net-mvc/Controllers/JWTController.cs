using System;
using System.Collections.Generic;
using System.Web.Http;
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
                    
                    string hstoken = Jose.JWT.Encode(payload, App_Start.StartUp.SecretByteKey, JwsAlgorithm.HS256);
                    string hsjson = Jose.JWT.Decode(hstoken, App_Start.StartUp.SecretByteKey);
                    return Ok(hsjson);

                case "RS256":
                    

                    string rstoken = Jose.JWT.Encode(payload, App_Start.StartUp.publicAndPrivate, JwsAlgorithm.RS256);
                    string rsjson = Jose.JWT.Decode(rstoken, App_Start.StartUp.publicAndPrivate);
                    return Ok(rsjson);

                case "ES256":

                    byte[] x = Authorization.GetBytes("BggqhkjOPQMBBw==");
                    byte[] y =
                        Authorization.GetBytes(
                            "MHcCAQEEIIe9/onbNPKUMM9gvBKE/nzAYIKV831MNFwQc/QUrjQcoAoGCCqGSM49AwEHoUQDQgAEqiLqYVFWTH/L5ejmjQyOJSvl1I2bFdCxWz8BXHAjp0afjMGPalOKoCvDD13FBb34O6InAAHzA4uZ3X2JjvXDag==");
                    byte[] d = Authorization.GetBytes("MHcCAQEEIIe9/onbNPKUMM9gvBKE/nzAYIKV831MNFwQc/QUrjQcoAoGCCqGSM49AwEHoUQDQgAEqiLqYVFWTH/L5ejmjQyOJSvl1I2bFdCxWz8BXHAjp0afjMGPalOKoCvDD13FBb34O6InAAHzA4uZ3X2JjvXDag==");

                    var privateKey = EccKey.New(x, y, d);

                    string estoken = Jose.JWT.Encode(payload, privateKey, JwsAlgorithm.ES256);
                    string esjson = Jose.JWT.Decode(estoken, privateKey);
                    return Ok(esjson);

                case "none":
                    
                    string nantoken = Jose.JWT.Encode(payload, null, JwsAlgorithm.none);
                    string nanjson = Jose.JWT.Decode(nantoken, null);
                    return Ok(nanjson);

                default:
                    return Ok();
            }

        }
    }
    public class Authorization 
    { 
        public static byte[] GetBytes(string input) 
        { 
            var bytes = new byte[input.Length * sizeof(char)]; 
            Buffer.BlockCopy(input.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;   
        } 
    }
}
