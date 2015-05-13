using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using asp.net_mvc.Models;
using asp.net_mvc.App_Start;

namespace asp.net_mvc.Controllers
{
    public class TemplateController : Controller
    {
        // GET: Template
        public ActionResult Index()
        {
            return View(StartUp.TemplateData);
        }
    }
}