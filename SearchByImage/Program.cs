using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace SearchByImage
{
    class Program
    {
        private const string URL = "http://localhost:14430/api/v1/afis/";
        private const string DATA = @"{""object"":{""name"":""Name""}}";

        static void Main(string[] args)
        {
            SearchByImageAsync(10).Wait();
        }

        static async Task SearchByImageAsync(int maxNumberOfCoincidences)
        {
            using (var client = new HttpClient())
            {
                byte[] image;
                image = LoadImage(@"C:\images\didac1.jpg");
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
                {
                    content.Add(new StreamContent(new MemoryStream(image)), $"fs-{DateTime.Now.ToString("yyyyMMddhhmmsss")}", $"fs-{DateTime.Now.ToString("yyyyMMddhhmmsss")}.jpg");
                    HttpResponseMessage response = await client.PostAsync($"searchByImages/{maxNumberOfCoincidences}/", content);
                    if (response.IsSuccessStatusCode)
                    {
                        string recordsJson = await response.Content.ReadAsStringAsync();
                        List<Record> records = JsonConvert.DeserializeObject<List<Record>>(recordsJson);
                        Console.WriteLine(records.Count());
                    }
                    else
                    {
                        Console.WriteLine("Internal server Error");
                    }
                }                
            }
            Console.ReadKey();
        }


        static async Task SearchByCodeAsync(string code)
        {
            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(URL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"getUserInfo/{code}/");
                if (response.IsSuccessStatusCode)
                {
                    string userJson = await response.Content.ReadAsStringAsync();
                    User user = JsonConvert.DeserializeObject<User>(userJson);
                    Console.WriteLine($"Nombres: {user.Name} {user.LastName}");
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
            Console.ReadKey();
        }

        static byte[] LoadImage(string filename)
        {
            Image img = Image.FromFile(filename);
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public class Record
        {
            public string Subject { get; set; }
            public double Match { get; set; }           
        }

        public class User
        {
            public bool Active { get; set; }
            public string UserCode { get; set; }
            public string Name { get; set; }
            public string LastName { get; set; }
            public DateTime BioLastUpdate { get; set; }
            public int BioUpdates { get; set; }
            public DateTime LastUpdate { get; set; }
            public DateTime Created { get; set; }
            public DateTime ExpiryDate { get; set; }
            public string Group { get; set; }
            public string Comments { get; set; }
            public byte[] Photo { get; set; }            
        }
    }
}
