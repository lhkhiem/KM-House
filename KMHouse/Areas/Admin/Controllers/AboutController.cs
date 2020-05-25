using Common;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMHouse.Areas.Admin.Controllers
{
    public class AboutController : BaseController
    {
        //[HasCredential(RoleID = "ABOUT_VIEW")]
        public ActionResult Index()
        {
            ViewBag.MenuActive = "mIndexAbout";
            var dao = new AboutDao().GetAbout();
            return View(dao);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(About entity)
        {
            var dao = new AboutDao();
            entity.CreateBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;
            entity.ModifiedBy = ((UserLoginSession)Session[ConstantSession.USER_SESSION]).UserName;

            dao.Update(entity);
            return View(entity);
        }
        public JsonResult ConvertString(string str)
        {
            string strConvert = StringHelper.ToUnsignString(str);
            return Json(new
            {
                str = strConvert
            }, JsonRequestBehavior.AllowGet);
        }
    }
}