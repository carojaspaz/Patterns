using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using CLNist.Archivo;
using Newtonsoft.Json;

namespace SearchByFingerPrint.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {           
            return View();
        }

        public ActionResult Image()
        {            
            byte[] img = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/Images"), "Prueba.wsq"));
            Wsq2Bmp.WsqDecoder decoder = new Wsq2Bmp.WsqDecoder();
            var bMap = decoder.Decode(img);
            var bitmapBytes = BitmapToBytes(bMap); //Convert bitmap into a byte array
            return File(bitmapBytes, "image/jpeg"); //Return as file result
        }

        public ActionResult ImageFromByte(byte[] image)
        {            
            return File(image, "image/jpeg"); //Return as file result
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public async Task<ActionResult> About()
        {
            string url = "http://172.28.45.207:10100/api/v1/abis/getResponseNist/0/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    Nist nist = JsonConvert.DeserializeObject<CLNist.Archivo.Nist>(responseData);
                    return View(nist);
                }
            }
            return View();            
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class FingerPrint
    {
        public byte[] print { get; set; }
    }
}