using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace asp.net_mvc.Models
{
    public class JWT
    {
        public static RsaKeyGenerationResult GenerateRsaKeys()
        {
            RSACryptoServiceProvider myRSA = new RSACryptoServiceProvider(2048);
            RSAParameters publicKey = myRSA.ExportParameters(true);
            RsaKeyGenerationResult result = new RsaKeyGenerationResult();
            result.PublicAndPrivateKey = myRSA.ToXmlString(true);
            result.PublicKeyOnly = myRSA.ToXmlString(false);
            return result;
        }

    }
 
public class RsaKeyGenerationResult
{
    public string PublicKeyOnly { get; set; }
    public string PublicAndPrivateKey { get; set; }
}
}