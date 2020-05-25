using Common;
using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace KMHouse.Areas.Admin.Controllers
{
    public class SliderController : Controller
    {
        [HasCredential(RoleID = "SLIDE_VIEW")]
        public ActionResult Index()
        {
            ViewBag.MenuActive = "mIndexSlider";
            return View();
        }
        public JsonResult LoadData(int type, string keyword, int pageIndex, int pageSize)
        {
            string str = NonUnicode.RemoveUnicode(keyword).ToLower();
            var model = new SliderDao().ListAll();
            switch (type)
            {
                case 0: model = model.Where(x => NonUnicode.RemoveUnicode(x.Name.ToLower()).Contains(str)); break;
            }

            int totalRow = model.Count();
            model = model.OrderBy(x => x.DisplayOrder)
              .Skip((pageIndex - 1) * pageSize)
              .Take(pageSize);
            int totalRowCurent = model.Count();

            return Json(new
            {
                data = model,
                total = totalRow,
                totalCurent = totalRowCurent,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [HasCredential(RoleID = "SLIDE_UPDATE")]
        public JsonResult SaveData(string strSlide)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            Slide slide = serializer.Deserialize<Slide>(strSlide);
            bool status = false;
            string action = string.Empty;
            string message = string.Empty;
            bool res = new SliderDao().CheckIDExist(slide.ID);
            if (res)
            {
                var model = new SliderDao();
                try
                {
                    model.Insert(slide);
                    status = true;
                    action = "insert";
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }
            }
            else
            {
                //update existing DB
                //save db
                try
                {
                    var model = new SliderDao().Update(slide);
                    status = true;
                    action = "update";
                }
                catch (Exception ex)
                {
                    status = false;
                    message = ex.Message;
                }

            }

            return Json(new
            {
                status = status,
                message = message,
                action = action
            });
        }
        [HasCredential(RoleID = "SLIDE_EDIT")]
        public JsonResult GetDetail(int id)
        {
            var model = new SliderDao().GetByID(id);
            return Json(new
            {
                data = model,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
        [HasCredential(RoleID = "SLIDE_DELETE")]
        public JsonResult Delete(int id)
        {
            var res = new SliderDao().Delete(id);
            if (res)
            {
                return Json(new
                {
                    status = true
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    status = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ChangeStatus(int id)
        {
            var res = new SliderDao().ChangeStatus(id);
            return Json(new
            {
                status = res
            });
        }
        public JsonResult ChangeOrder(int id, int order)
        {
            var res = new SliderDao().ChangeOrder(id, order);
            if (res)
            {
                return Json(new
                {
                    status = true
                });
            }
            else
            {
                return Json(new
                {
                    status = false
                });
            }

        }
    }
}