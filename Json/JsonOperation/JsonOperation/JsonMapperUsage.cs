using LitJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonOperation
{
    class JsonMapperUsage
    {
        /// <summary>
        /// generate the object according the schema definition
        /// will throw exception once failed
        /// </summary>
        /// <param name="jsonString"></param>
        /// <returns></returns>
        public static Object ParseJson()
        {
            //string jsonString  = @"{\"schema\":\"WrapStar[2.0]\",\"Models\":[{\"Id\":\"10679\",\"Name\":\"Food\",\"Latest\":true,\"Timestamp\":\"20141023T063426\",\"Version\":\"10679\",\"Entities\":[{\"Type\":\"Food\",\"Properties\":[{\"Type\":\"Food.Recipe.Title\",\"Value\":[\"Deep Fried Black Eyed Peas\"]}]}]}]}";
            string jsonString = File.ReadAllText(@"D:\testJson.txt");
            //LitJson can not tolerate character '.' in the name so we replace it with schema
            jsonString = jsonString.Replace("Kif.Schema", "schema");
            return LitJson.JsonMapper.ToObject<WrapStarDataDefinition.WrapStarSchema>(jsonString);
        }

        public static string GenerateJson(Object obj)
        {
            //Unhandled Exception: LitJson.JsonException: Max allowed object depth reached while trying to export from type System.Single
            return LitJson.JsonMapper.ToJson(obj);
        }

    }


    //Definition of the WrapStar Kif according to the schema
    class WrapStarDataDefinition
    {
        public class WrapStarProperty
        {
            public string Type;
            public float Score;
            public List<string> Value;
            public List<WrapStarEntity> Entities;
        }

        public class WrapStarEntity
        {
            public string Type;
            public float Score;
            public List<WrapStarProperty> Properties;
        }

        public class WrapStarModel
        {
            public string Id;
            public string Name;
            public bool Latest;
            public string Timestamp;
            public List<WrapStarEntity> Entities;
            public string Version;
        }

        public class WrapStarSchema
        {
            //kif schema
            public string schema;
            public List<WrapStarModel> Models;
        }
    }
}
