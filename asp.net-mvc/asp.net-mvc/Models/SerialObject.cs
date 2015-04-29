using System.Collections.Generic;

namespace asp.net_mvc.models
{
    public class SerialObject
    {
        public int Id { get; set; }

        public int? aNull { get; set; }

        public bool aBoolean { get; set; }

        public int anInteger { get; set; }

        public float aFloat { get; set; }

        public string aString { get; set; }

        public List<string> anArray { get; set; }

        public TestObject anObject { get; set; }

        //{"Id":0,"aNull":null,"aBoolean":true,"anInteger":68523,"aFloat":685230.15,
        //"aString":"Testing s","anArray":[null,true,123,"any"],
        //"anObject":{"aNull":null,"aBoolean":true,"anInteger":123,"aString":"any"}}
    }

    public class TestObject
    {
        public int? aNull { get; set; }

        public bool aBoolean { get; set; }

        public int anInteger { get; set; }

        public string aString { get; set; }
    }
}