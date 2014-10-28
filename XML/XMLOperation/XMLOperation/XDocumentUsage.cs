using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace XMLOperation
{

    /*
     * XDocument - 表示一个 XML 文档
     * XElement - 表示一个 XML 元素
     * XName - 表示元素 (XElement) 和属性 (XAttribute) 的名称
     * XAttribute  - 表示一个 XML 属性
     * XCData - 表示一个 CDATA 文本节点
     * XComment - 表示一个 XML 注释
     
     
     */
    class XDocumentUsage
    {
        public static void XDocumentOperation()
        { 
            //1. load or parse data
            //1.1 load
            XDocument xDoc = null;
            //string inputxmlfilename = "";
            //xDoc = XDocument.Load(inputxmlfilename);

            //1.2 parse 
            string xmlConent = @"<contact>
                                    test content many contents so many contents
                                    <id>cppbuilder</id>
                                    <blogUrl>http://bcb.tw/blog</blogUrl>
                                    <email provider='x'>x@bcb.tw</email>
                                    <email provider='yahoo'>iapx_432@yahoo.com.tw</email>
                                </contact>";
            xDoc = XDocument.Parse(xmlConent);

            //2. Traversal
            //2.1 Traversal Element
            XElement pitchers = xDoc.Root;
            foreach (XElement p in pitchers.Elements())
                Console.WriteLine(p);
            Console.WriteLine("------Element vs Node-----"); // Node contain the text which no tag arround it but element cannot
            //2.2 Traversal Nodes
            foreach (var c in pitchers.Nodes())
                Console.WriteLine(c);

            //3. search
            var childnodeCollection = pitchers.Elements("email");
 
            var childnode = pitchers.Element("id");
 
            var descendantNodeCollection = pitchers.Descendants("blogUrl");
 

            //4. add
            pitchers.Add(new XElement("pitcher",
                    new XElement("name", "J.Santana"),
                    new XElement("wins", 19),
                    new XElement("team", "MIN")
                    )
                    );
            Console.WriteLine(pitchers.ToString());
            //5. modify Attribute
            pitchers.Element("id").SetAttributeValue("class","red");
            Console.WriteLine("id : " + pitchers.Element("id").Attribute("class").Value);
            pitchers.Element("id").Remove();
        }

        public static void LINQToXDocumentOperation()
        {
            var pitcherList = new[] {
                        new Pitcher{ Name = "C Wang", Wins = 19, Team = "NYY"},
                        new Pitcher{ Name = "R.Johnson", Wins = 17,Team = "NYY"},
                        new Pitcher{ Name = "R.Halladay", Wins = 16, Team = "TOR"}};

            XElement pitchersXml = new XElement("pitchers",
                                    from p in pitcherList
                                    where p.Team == "NYY"
                                    select new XElement("pitcher",
                                    new XElement("name", p.Name),
                                    new XElement("wins", p.Wins),
                                    new XElement("team", p.Team)
                                    )
                                    );

            XElement wins17 = new XElement("wins17",
                from p in pitchersXml.Elements("pitcher")
                where int.Parse((string)p.Element("wins")) >= 17
                select new object[] {
                    new XElement("name", (string)p.Element("name"))
                });
            Console.WriteLine(wins17);


            XElement wins17_2 = new XElement("wins17",
                from p in pitchersXml.Elements("pitcher")
                orderby int.Parse((string)p.Element("wins"))
                select new object[] {
                    new XElement(
                        "pitcher", 
                        (string)p.Element("name"), 
                        new XAttribute("wins", (string)p.Element("wins"))
                    )
                });
            Console.WriteLine(wins17_2);

        }  
        
    }
    
    class Pitcher
    {
        public string Name;
        public int Wins;
        public string Team;
    }
}
