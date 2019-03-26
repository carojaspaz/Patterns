using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;

namespace SearchByFingerPrint.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {           
            return View();
        }

        public ActionResult Image(string formatImage, byte[] array)
        {            
            byte[] img = System.IO.File.ReadAllBytes(Path.Combine(Server.MapPath("~/Images"), "Prueba.wsq"));
            Wsq2Bmp.WsqDecoder decoder = new Wsq2Bmp.WsqDecoder();
            var bMap = decoder.Decode(img);
            var bitmapBytes = BitmapToBytes(bMap); //Convert bitmap into a byte array
            return File(bitmapBytes, "image/jpeg"); //Return as file result
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

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