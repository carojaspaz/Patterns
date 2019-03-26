using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DPUruNet;

namespace SearchByFingerPrint.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //var _readers = ReaderCollection.GetReaders();
            //List<SelectListItem> readers = _readers.Select(m => new SelectListItem { Text = m.Description.Name, Value = m.Description.SerialNumber }).ToList();
            //ViewBag.readers = readers;
            return View();
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

    public class ReaderItem
    {
        public string Name { get; set; }
        public string Serial { get; set; }
    }
}