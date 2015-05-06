using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using StackExchange.Redis;

namespace asp.net_mvc.App_Start
{
    public class StartUp
    {
        public static Array Indexes { get; set; }
        public static Decimal NumberOfRoutes { get; set; }
        public static Dictionary<int, string> JSONData { get; set; } 

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