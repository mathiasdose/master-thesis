using System.Web.Mvc;
using asp.net_mvc.App_Start;

namespace asp.net_mvc.Controllers
{
    public class TemplateController : Controller
    {
        public ActionResult Index()
        {
            return View(StartUp.TemplateData);
        }
    }
}