using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonOperation
{
    //http://james.newtonking.com/json/help/index.html

    /*
     *JObject-用于操作JSON对象
     *JArray-用于操作JSON数组
     *JValue-表示数组中的值
     *JProperty-表示对象中的属性,以"key/value"形式
     *JToken-用于存放Linq to JSON查询后的结果   
     */


    /*
    JObject.Parse(string json)       //json含有JSON对象的字符串，返回为JObject对象 
    JObject.FromObject(object o)     //o为要转化的对象，返回一个JObject对象
    JObject.Load(JsonReader reader)  //reader包含着JSON对象的内容，返回一个JObject对象 
    * */

    class JObjectUsage
    {
        /// <summary>
        /// usage 1: JObject.Parse()
        /// usage 2: JObject[]
        /// usage 3: JArray
        /// </summary>
        /// <param name="kifCDATAOutput"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ModifyJsonString(string kifCDATAOutput, string url)
        {
            // add DocUrl in each tab in Tabs list
            JObject jobj = JObject.Parse(kifCDATAOutput);
            var tabsJArr = (JArray)jobj["Tabs"];
            foreach (var tabJobj in tabsJArr)
            {
                if (tabJobj["DocURL"] == null || string.IsNullOrEmpty(tabJobj["DocURL"].ToString()))
                {
                    tabJobj["DocURL"] = url;
                }
            }
            string newOutput = jobj.ToString();
            return newOutput;
        }



        public static void JsonOperation()
        { 
            //1. create JObject
            JObject staff = new JObject();
            staff.Add(new JProperty("Name", "Jack"));
            staff.Add(new JProperty("Age", 33));
            Console.WriteLine(staff.ToString());

            //2. access JObject attribute
            JToken ageToken = staff["Age"];
            Console.WriteLine(ageToken.ToString());

            //3. modify attriute value
            staff["Age"] = 45;

            //4. remove attribute 
            staff.Remove("Age");

            //5. append attribute
            staff["Name"].Parent.AddAfterSelf(new JProperty("Department", "Personnel Department"));
       
        }
        
        public static void JsonOperation2()
        {
            //1. Parse json 
            string json = "{\"Name\" : \"Jack\", \"Age\" : 34, \"Colleagues\" : [{\"Name\" : \"Tom\" , \"Age\":44},{\"Name\" : \"Abel\",\"Age\":29}] }";
            JObject jObj = JObject.Parse(json);

            //2. select
            var names = from colleague in jObj["Colleagues"].Children()  //"Children()"可以返回所有数组中的对象
                        select (string)colleague["Name"];

            foreach (var name in names)
                Console.WriteLine(name);
            
            //3. add
            JObject linda = new JObject(new JProperty("Name", "Linda"), new JProperty("Age", "23"));
            jObj["Colleagues"].Last.AddAfterSelf(linda);

            //4. remove
            jObj["Colleagues"][1].Remove();
            Console.WriteLine(jObj.ToString());

            // 5. select using SelectToken
            //5.1 利用SelectToken来查询名称
            JToken selectname = jObj.SelectToken("Name");
            Console.WriteLine(selectname.ToString());

            //5.2 查询所有同事的名字
            var selectnames = jObj.SelectToken("Colleagues").Select(p => p["Name"]).ToList();
            foreach (var name in selectnames)
                Console.WriteLine(name.ToString());

            //5.3 查询最后一名同事的年龄
            var age = jObj.SelectToken("Colleagues[1].Age");
            Console.WriteLine(age.ToString());

        }
           
    }
}
