using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonOperation
{
    public class GoogleAuthProperty
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }


    [JsonObject(MemberSerialization.OptOut)]
    public class Book
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public string Author { get; set; }  //不想被序列化成字串,可以使用 [JsonIgnore] 去忽略
    }


    class JsonConvertCustomClassUsage
    {
        public static void TestGoogleAuthProperty()
        {
            //1. deserialize
            string responseFromServer = "{\"access_token\" : \"xxxxxxxxxxxxxxxxxxxxxxxxxxx\",\"token_type\" : \"Bearer\", \"expires_in\" : 3600, \"refresh_token\" : \"yyyyyyyyyyyyyyyyyyyyyyy\"}";
            var prop = JsonConvert.DeserializeObject<GoogleAuthProperty>(responseFromServer);
            Console.WriteLine("AccessToken "+ prop.AccessToken);

            //2. serialize
            var serializeData = JsonConvert.SerializeObject(prop);
            Console.WriteLine("serializeData " + serializeData);
        }

        public static void TestBook()
        {
            //1. deserialize
            string data = "{\"ID\" : \"10\",\"Name\" : \"LLCC\"}";
            var prop = JsonConvert.DeserializeObject<Book>(data);
            Console.WriteLine("Name " + prop.Name);

            //2. serialize
            var serializeData = JsonConvert.SerializeObject(prop);
            Console.WriteLine("serializeData " + serializeData);
        }

    }
}
