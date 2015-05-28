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
        public static world WorldObject { get; set; }
        public static Dictionary<int, string> JSONData { get; set; }
        public static List<TemplateEngine> TemplateData { get; set; }
        public static Dictionary<int, List<TemplateEngine>> TemplateDData { get; set; }
        public static string SecretKey { get; set; }
        public static byte[] SecretByteKey { get; set; }
        public static byte[] PrivateRSAKey { get; set; }
        public static byte[] PublicRSAKey { get; set; }
        public static byte[] PrivateECKey { get; set; }
        public static byte[] PublicECKey { get; set; }
        public static RSACryptoServiceProvider publicAndPrivate { get; set; }
        public static byte[] EccX { get; set; }
        public static byte[] EccY { get; set; }
        public static byte[] EccD { get; set; }
        public static Array CacheKeys { get; set; }

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

            EccX = new byte[]{ 4, 114, 29, 223, 58, 3, 191, 170, 67, 128, 229, 33, 242, 178, 157, 150, 133, 25, 209, 139, 166, 69, 55, 26, 84, 48, 169, 165, 67, 232, 98, 9 };
            EccY = new byte[]{ 131, 116, 8, 14, 22, 150, 18, 75, 24, 181, 159, 78, 90, 51, 71, 159, 214, 186, 250, 47, 207, 246, 142, 127, 54, 183, 72, 72, 253, 21, 88, 53 };
            EccD = new byte[] { 42, 148, 231, 48, 225, 196, 166, 201, 23, 190, 229, 199, 20, 39, 226, 70, 209, 148, 29, 70, 125, 14, 174, 66, 9, 198, 80, 251, 95, 107, 98, 206 };
        }

        private static void SetupTemplate()
        {
            using (var fs = new StreamReader(HostingEnvironment.ApplicationPhysicalPath + @".\data\filtered.json"))
            {
                string text = fs.ReadToEnd();
                TemplateData = JsonConvert.DeserializeObject<List<TemplateEngine>>(text);
            }
            TemplateDData = new Dictionary<int, List<TemplateEngine>>();
            
            TemplateDData.Add(10, TemplateData.GetRange(0, 10).ToList());
            TemplateDData.Add(100, TemplateData);
            
            TemplateDData.Add(1000, Times(TemplateData, 10));
            TemplateDData.Add(10000, Times(TemplateData, 100));

        }

        private static List<TemplateEngine> Times(List<TemplateEngine> list, int n)
        {
            var copy = new List<TemplateEngine>();
            for (int i = 0; i < n; i++)
            {
                copy.AddRange(list);
            }
            return copy;
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
                var keys = new List<string>();
                for (int i = 0; i < 100; i++)
                {
                    HashEntry[] hash = { new HashEntry("field1", "Hello"), new HashEntry("field2", "World" + i) };
                    var key = "Hash:" + i;
                    cacheDatabase.HashSet(key, hash);
                    keys.Add(key);
                }
                CacheKeys = keys.ToArray();
            }
        }

        private static void SetupJSONTest()
        {
            
            var paths = Directory.GetFiles(HostingEnvironment.ApplicationPhysicalPath + @".\data\", "*0.json" );

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
            var worlds = new List<world>();
            WorldObject = new world()
            {
                randomInteger = 10,
                randomString = "Hello world",
                randomDecimal = new Decimal(0.5),
                randomDate = new DateTime()
            };
            for (var i = 0; i < 100; i++)
            {
                worlds.Add(new world()
                {
                    randomInteger = 10,
                    randomString = "Hello world",
                    randomDecimal = new Decimal(0.5),
                    randomDate = new DateTime()
                });
            }

            
            return worlds;
        }
    }
}