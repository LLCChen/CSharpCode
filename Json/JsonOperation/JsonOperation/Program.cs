using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonOperation
{
    class Program
    {
        static void Main(string[] args)
        {
            //JsonConvertCustomClassUsage.TestGoogleAuthProperty();
            //JsonConvertCustomClassUsage.TestBook();
            var obj = JsonMapperUsage.ParseJson();
            Console.WriteLine(obj.ToString());
            Console.WriteLine("=====");
            Console.WriteLine(JsonMapperUsage.GenerateJson(obj));

        }
    }
}
