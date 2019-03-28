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
using System.Web.Helpers;
using SearchByFingerPrint.Models;

namespace SearchByFingerPrint.Controllers
{
    enum DedoMano
    {
        ManoDerecha_Pulbar = 1,
        Manoderecha_Indice,
        ManoDerecha_Medio,
        ManoDerecha_Anular,
        ManoDerecha_Menique,
        ManoIzquierda_Pulgar,
        ManoIzquierda_Indice,
        ManoIsquierda_Medio,
        ManoIsquierda_Anular,
        ManoIzquierda_Menique
    }
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
            ViewBag.GetFinger = new Func<string,string>(GetFinger);
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
                    int dedo = int.Parse(nist.RegistrosTipo14.First().FGP);
                    DedoMano dedoMano = (DedoMano)dedo;
                    ViewBag.DedoMano = dedoMano.ToString();
                    return View(nist);
                }
            }
            return View();            
        }
        
        public string GetFinger(string finger)
        {
            switch (finger)
            {
                case "1":
                    return "Pulgar Mano Derecha";
                case "2":
                    return "Indice Mano Derecha";
                case "3":
                    return "Medio Mano Derecha";
                case "4":
                    return "Anular Mano Derecha";
                case "5":
                    return "Meñique Mano Derecha";
                case "6":
                    return "Pulgar Mano Izquierda";
                case "7":
                    return "Indice Mano Izquierda";
                case "8":
                    return "Medio Mano Izquierda";
                case "9":
                    return "Anular Mano Izquierda";
                case "10":
                    return "Meñique Mano Izquierda";
            }
            return string.Empty;
        }

        public async Task<ActionResult> Contact(string requestId)
        {
            string url = $@"http://172.28.45.207:10100/api/v1/abis/getResponseFile/{requestId}/";
            ViewBag.GetFinger = new Func<string, string>(GetFinger);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync(url);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var nistResponse = JsonConvert.DeserializeObject<GetResponseFile>(responseData);
                    //TO-DO: Evaluar el estado
                    CLNist.Archivo.Nist nist = new Nist();
                    nist.Leer(nistResponse.NistFile);
                    return View(nist);
                }
            }
            return View(new CLNist.Archivo.Nist());
        }
    }

    public class FingerPrint
    {
        public byte[] print { get; set; }
    }
}