using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel.Security.Tokens;
using System.Web;
using System.Web.Http;
using asp.net_mvc.Models;
using Jose;
using Security.Cryptography;
using JWT = asp.net_mvc.Models.JWT;

namespace asp.net_mvc.Controllers
{
    public class DeprJWT : ApiController
    {
        public IHttpActionResult GetJWT(string type)
        {
            var tokenHandler = new JwtSecurityTokenHandler();


            SecurityToken issuerSecurityToken = null;
            JwtHeader head = new JwtHeader();

            switch (type)
            {
                case "HS256":
                    byte[] securityKey =
                        global::asp.net_mvc.Controllers.Authorization.GetBytes("0rtfaE3N58pPkQ7UURL6H4D4Ostht0N1");

                    head = new JwtHeader(new SigningCredentials(
                        new InMemorySymmetricSecurityKey(securityKey),
                        SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest));

                    issuerSecurityToken = new BinarySecretSecurityToken(securityKey);

                    var hspayload = new Dictionary<string, object>()
                    {
                        {"iss", "John Doe"}
                    };

                    string hstoken = Jose.JWT.Encode(hspayload, securityKey, JwsAlgorithm.HS256);
                    string hsjson = Jose.JWT.Decode(hstoken, securityKey);
                    return Ok(hsjson);
                    break;

                case "RS256":
                    RSACryptoServiceProvider publicAndPrivate = new RSACryptoServiceProvider();
                    RsaKeyGenerationResult keyGenerationResult = JWT.GenerateRsaKeys();

                    publicAndPrivate.FromXmlString(keyGenerationResult.PublicAndPrivateKey);

                    head = new JwtHeader(new SigningCredentials(
                        new RsaSecurityKey(publicAndPrivate)
                        , SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest));

                    issuerSecurityToken = new RsaSecurityToken(publicAndPrivate);

                    var rspayload = new Dictionary<string, object>()
                    {
                        {"iss", "John Doe"}
                    };

                    string rstoken = Jose.JWT.Encode(rspayload, publicAndPrivate, JwsAlgorithm.RS256);
                    string rsjson = Jose.JWT.Decode(rstoken, publicAndPrivate);
                    return Ok(rsjson);
                    break;

                case "ES256":
                    var espayload = new Dictionary<string, object>()
                    {
                        {"iss", "John Doe"}
                    };
                    byte[] x =
                    {
                        4, 114, 29, 223, 58, 3, 191, 170, 67, 128, 229, 33, 242, 178, 157, 150, 133, 25, 209,
                        139, 166, 69, 55, 26, 84, 48, 169, 165, 67, 232, 98, 9
                    };
                    byte[] y =
                    {
                        131, 116, 8, 14, 22, 150, 18, 75, 24, 181, 159, 78, 90, 51, 71, 159, 214, 186, 250, 47,
                        207, 246, 142, 127, 54, 183, 72, 72, 253, 21, 88, 53
                    };
                    byte[] d =
                    {
                        42, 148, 231, 48, 225, 196, 166, 201, 23, 190, 229, 199, 20, 39, 226, 70, 209, 148, 29,
                        70, 125, 14, 174, 66, 9, 198, 80, 251, 95, 107, 98, 206
                    };

                    var privateKey = EccKey.New(x, y, d);

                    string estoken = Jose.JWT.Encode(espayload, privateKey, JwsAlgorithm.ES256);
                    string esjson = Jose.JWT.Decode(estoken, privateKey);
                    return Ok(esjson);

                case "none":
                    head = new JwtHeader();
                    head.Add("type", "JWT");
                    head.Add("alg", "none");

                    var nanpayload = new Dictionary<string, object>()
                    {
                        {"iss", "John Doe"}
                    };


                    string nantoken = Jose.JWT.Encode(nanpayload, null, JwsAlgorithm.none);
                    string nanjson = Jose.JWT.Decode(nantoken, null);
                    return Ok(nanjson);
                    break;

                default:
                    return Ok();
            }


            var load = new JwtPayload {{"iss", "John Doe"}};

            if (issuerSecurityToken == null)
            {
                issuerSecurityToken = new JwtSecurityToken(head, load);

            }

            var tk = new JwtSecurityToken(head, load);

            var tkString = tokenHandler.WriteToken(tk);

            var validationParameters = new TokenValidationParameters()
            {
                IssuerSigningToken = issuerSecurityToken,
                ValidIssuer = "John Doe",
            };
            validationParameters.ValidateActor = false;
            validationParameters.ValidateLifetime = false;
            validationParameters.ValidateAudience = false;


            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(tkString, validationParameters, out securityToken);
            var userData = principal.Claims.FirstOrDefault();



            /*
            
            JwtSecurityToken jwtToken = new JwtSecurityToken
                (issuer: "http://issuer.com", audience: "http://mysite.com"
                , signingCredentials: new SigningCredentials(new RsaSecurityKey("RSA key")
                    , SecurityAlgorithms.RsaSha256Signature, SecurityAlgorithms.Sha256Digest));

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string tokenString = tokenHandler.WriteToken(jwtToken);

            Console.WriteLine("Token string: {0}", tokenString);*/

            return Ok(new {userData});
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

    
