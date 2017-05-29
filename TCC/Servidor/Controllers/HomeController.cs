using Servidor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Servidor.Controllers
{
    public class HomeController : Controller
    {
        private static GPIOControl GPIOs= new GPIOControl();

        public ActionResult Index()
        {
            var ports = new List<bool>();
            GPIOs.Ports.ForEach(c => ports.Add(c.PortStatus));
            return View(ports);
        }

        [HttpPost]
        public ActionResult TogglePort(int portNum)
        {
            GPIOs.Ports[portNum].PortStatus = !GPIOs.Ports[portNum].PortStatus;

            return Json(GPIOs.Ports[portNum].PortStatus);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult getGPIOPortStatus()
        {
            var ports = new List<bool>();
            GPIOs.Ports.ForEach(c=>ports.Add(c.PortStatus));
            return Json(ports, JsonRequestBehavior.AllowGet);
        }
    }
}