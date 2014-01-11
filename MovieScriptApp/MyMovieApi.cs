using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using MovieScriptApp;

namespace ConsoleAppScriptForMovie
{
    public class MyMovieApi
    {
        public static string Process(string filetowritedownloadeddatato, string movieId)
        {
            //System.IO.File.WriteAllLines(@"C:\Users\PrashMaya\Desktop\WriteFirst50Lines.txt", titleIds.ToArray());
            using (var client = new HttpClient())
            {
                string url = "http://mymovieapi.com/?ids={0}&type=json&plot=full&episode=1&lang=en-US&aka=simple&release=simple&business=0&tech=0";
                string anotherurl = String.Format(url, movieId);
                client.BaseAddress = new Uri(anotherurl);
                HttpResponseMessage anotherresponse = client.GetAsync(anotherurl).Result;
                object obj = JsonConvert.DeserializeObject<object>(anotherresponse.Content.ReadAsStringAsync().Result);

                dynamic moreInfo = JsonConvert.DeserializeObject(obj.ToString());
                if (obj.ToString() == "[]")
                    return obj.ToString();
                string temp = moreInfo[0]["plot"];
                List<string> tempString = new List<string>();
                tempString.Add(obj.ToString());
                System.IO.File.WriteAllLines(filetowritedownloadeddatato, tempString);
                return obj.ToString();
            }

            string JSONText = "{Text:\"Hello World!!\", Status:\"Ok\"}";
            JsonSerializer js = new JsonSerializer();
            var x = js.Deserialize(new StringReader(JSONText), typeof(HelloWorldResponse)) as HelloWorldResponse;
        }
    }

    public class HelloWorldResponse
    {
        public string Text { get; set; }

        public string Status { get; set; }
    }
}
