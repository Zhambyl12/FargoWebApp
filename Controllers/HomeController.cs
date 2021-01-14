using FargoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FargoWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {  
            return View(FargoData.GetCities());
        }

        public ActionResult test()
        {
            return View(FargoData.GetPrices1Async());
        }
    }
}