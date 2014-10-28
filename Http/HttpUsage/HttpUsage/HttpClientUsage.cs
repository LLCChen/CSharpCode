using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpUsage
{
    class HttpClientUsage
    {
        /// <summary>
        /// Fetch a page by using http request
        /// </summary>
        public static void Test_HttpWebRequest()
        {
            string urlRequest = ""; // input param

            string responseData = string.Empty;
            string userAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; WOW64; Trident/5.0)";
            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp(urlRequest);//WebRequest.Create(url) as HttpWebRequest;
                request.CookieContainer = new CookieContainer();
                request.Method = "GET";

                request.UserAgent = userAgent;
                request.UseDefaultCredentials = true;

                request.KeepAlive = true;
                request.Accept = @"*/*";

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    responseData = new StreamReader(response.GetResponseStream()).ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed while getting response for {0} with below error message", urlRequest));
                Console.WriteLine(ex.Message);
            }
        }



        /// <summary>
        ///  send a post request to a server and receive the response
        /// </summary>
        public static void Test_HttpClientHandlerUsage()
        {
            string answerData = "";  // input param

            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            using (var client = new HttpClient(handler))
            {
                var content = new FormUrlEncodedContent(
                    new[]
                    {
                        new KeyValuePair<string,string>("answerDataXml", answerData)
                    });
                var result = client.PostAsync("http://lsstchost02:8080/Preview", content).Result; // using Post-way to sent the request
                string resultDataStr = result.Content.ReadAsStringAsync().Result;
            }
        
        }


        /// <summary>
        /// send a get request to a server and receive the response
        /// </summary>
        public static void Test_HttpClient()
        {
            // need add these header. otherwise, the request will be rejected.
            const string USERAGENT = @"Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";           
            const string ACCEPT = @"text/html, application/xhtml+xml, */*";
            const string ACCEPTENCODING = @"gzip,deflate";
            const string REFERER = @"http://bingdex/#/";

            const string BINGDEXURL = @"http://bingdex/json/Data/RetroIndex?url=";

            List<string> queryUrlList = new List<string>();  // input param
            Dictionary<string, string> resultDict = new Dictionary<string, string>(); // output result
            string curQueryUrl = "";

            try
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.UseDefaultCredentials = true;
                handler.CookieContainer = new CookieContainer();
                handler.UseCookies = true;
                handler.AllowAutoRedirect = true;

                using (HttpClient client = new HttpClient(handler))
                {
                    // Add a user-agent header
                    client.DefaultRequestHeaders.Add("user-agent", USERAGENT);                     
                    // Add referer
                    client.DefaultRequestHeaders.Add("referer", REFERER);                     
                    // Add accept
                    client.DefaultRequestHeaders.Add("accept", ACCEPT);                     
                    // Add accept-encoding
                    client.DefaultRequestHeaders.Add("accept-encoding", ACCEPTENCODING);
                     
                    int i = 0;
                    foreach (string url in queryUrlList)
                    {
                        // compose the url and encoding the request request parameter rather than the whole request url
                        string requestUrl = string.Format("{0}{1}", BINGDEXURL, System.Web.HttpUtility.UrlEncode(url));
                        curQueryUrl = url;
                        try
                        {
                            // send request
                            var result = client.GetAsync(requestUrl).Result;   //GetAsync - Get way
                            string res = result.Content.ReadAsStringAsync().Result;
                            resultDict[url] = res;
                        }
                        catch (ArgumentNullException ae)
                        {
                            Console.WriteLine(string.Format("ArgumentNullException occur for {0} with below error message {1}", curQueryUrl, ae.StackTrace));
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }
                        i += 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed while getting response for {0} with below error message {1}", curQueryUrl, ex.StackTrace));
                Console.WriteLine(ex.Message);
            }

        }

    }
}
