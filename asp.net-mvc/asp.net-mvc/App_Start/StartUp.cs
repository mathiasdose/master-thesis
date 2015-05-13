using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using asp.net_mvc.Models;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace asp.net_mvc.App_Start
{
    public class StartUp
    {
        public static Array Indexes { get; set; }
        public static Decimal NumberOfRoutes { get; set; }
        public static Dictionary<int, string> JSONData { get; set; }
        public static List<TemplateEngine> TemplateData { get; set; }
        public static string SecretKey { get; set; }
        public static byte[] SecretByteKey { get; set; }
        public static byte[] PrivateRSAKey { get; set; }
        public static byte[] PublicRSAKey { get; set; }
        public static byte[] PrivateECKey { get; set; }
        public static byte[] PublicECKey { get; set; }
        public static RSACryptoServiceProvider publicAndPrivate { get; set; }

        public static void Init()
        {
            NumberOfRoutes = bool.Parse(ConfigurationManager.AppSettings["RequestTest"]) ? 
                decimal.Parse(ConfigurationManager.AppSettings["NumberOfRoutes"]) : 1;

            if (bool.Parse(ConfigurationManager.AppSettings["O*MTest"]))
                SetupDbTest();

            if (bool.Parse(ConfigurationManager.AppSettings["SerialTest"]))
                SetupJSONTest();

            if (bool.Parse(ConfigurationManager.AppSettings["CacheTest"]))
                SetupCacheTest();

            if (bool.Parse(ConfigurationManager.AppSettings["TemplateTest"]))
                SetupTemplate();

            if (bool.Parse(ConfigurationManager.AppSettings["JWTTest"]))
                SetupJWTTest();
            
            

        }

        private static void SetupJWTTest()
        {
            SecretKey = "0rtfaE3N58pPkQ7UURL6H4D4Ostht0N1";
            SecretByteKey = GetBytes(SecretKey);
            using (var fs = new StreamReader(HostingEnvironment.ApplicationPhysicalPath + @".\data\private.pem"))
            {
                string text = fs.ReadToEnd();
                PrivateRSAKey = GetBytes(text);
            }
            using (var fs = new StreamReader(HostingEnvironment.ApplicationPhysicalPath + @".\data\public.pem"))
            {
                string text = fs.ReadToEnd();
                PublicRSAKey = GetBytes(text);
            }
            using (var fs = new StreamReader(HostingEnvironment.ApplicationPhysicalPath + @".\data\privateec.pem"))
            {
                string text = fs.ReadToEnd();
                PrivateECKey = GetBytes(text);
            }
            using (var fs = new StreamReader(HostingEnvironment.ApplicationPhysicalPath + @".\data\cert.pem"))
            {
                string text = fs.ReadToEnd();
                PublicECKey = GetBytes(text);
            }

            publicAndPrivate = new RSACryptoServiceProvider();
            RsaKeyGenerationResult keyGenerationResult = JWT.GenerateRsaKeys();

            publicAndPrivate.FromXmlString(keyGenerationResult.PublicAndPrivateKey);
        }

        private static void SetupTemplate()
        {
            using (var fs = new StreamReader(HostingEnvironment.ApplicationPhysicalPath + @".\data\filtered.json"))
            {
                string text = fs.ReadToEnd();
                TemplateData = JsonConvert.DeserializeObject<List<TemplateEngine>>(text);
            }
        }

        private static byte[] GetBytes(string text)
        {
            var bytes = new byte[text.Length * sizeof(char)];
            Buffer.BlockCopy(text.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;   
        }

        private static void SetupCacheTest()
        {

            using (var redis = ConnectionMultiplexer.Connect("localhost"))
            {
                IDatabase cacheDatabase = redis.GetDatabase();
                
                for (int i = 0; i < 100; i++)
                {
                    HashEntry[] hash = { new HashEntry("field1", "Hello"), new HashEntry("field2", "World" + i) };
                    cacheDatabase.HashSet("Hash:" + i, hash);
                }
                
            }
        }

        private static void SetupJSONTest()
        {
            
            var paths = Directory.GetFiles(HostingEnvironment.ApplicationPhysicalPath + @".\data\", "*.json" );

            JSONData = new Dictionary<int, string>();
            foreach (var path in paths)
            {
                using (var fs = new StreamReader(path))
                {
                    string text = fs.ReadToEnd();
                    JSONData.Add(int.Parse(Path.GetFileNameWithoutExtension(path)), text);
                }
            }
        }

        private static void SetupDbTest()
        {
            
            using (var db = new MyDbContainer())
            {
                db.Database.CreateIfNotExists();

                db.world.RemoveRange(db.world);
                db.SaveChanges();

                db.world.AddRange(WorldData());
                db.SaveChanges();

                Indexes = db.world.Select(ent => ent.id).ToArray();
            }
        }

        private static IEnumerable<world> WorldData()
        {
            var random = new Random();
            var worlds = new List<world>();
            for (var i = 0; i < 100; i++)
            {
                worlds.Add(new world() {randomNumber = random.Next(0, 10000)});
            }

            return worlds;
        }
    }
}