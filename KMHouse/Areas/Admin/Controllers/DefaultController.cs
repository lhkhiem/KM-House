using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMHouse.Areas.Admin.Controllers
{
    public class DefaultController : BaseController
    {
        
        public ActionResult TestCheckbox()
        {
            
            return View();
        }
        public ActionResult Index()
        {
            @ViewBag.MenuActive = "mIndexDefault";
            return View();
        }
        public ActionResult Index2()
        {
            ViewBag.MenuActive = "mIndex";
            return View();
        }
        // GET: Admin/Default/Details/5
        public ActionResult Details()
        {
            ViewBag.MenuActive = "mDetails";
            return View();
        }

        
    }
}
