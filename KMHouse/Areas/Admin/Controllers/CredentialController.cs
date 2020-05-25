using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KMHouse.Areas.Admin.Controllers
{
    public class CredentialController : BaseController
    {
        public ActionResult Index(string ID)
        {
            ViewBag.MenuActive = "mEditCredential";
            if (string.IsNullOrEmpty(ID))
                return RedirectToAction("Index", "UserGroup");
            else
            {
                if (ID=="ADMIN")
                {
                    return RedirectToAction("Index", "UserGroup");
                }
                else
                {
                    ViewBag.UserGroup = ID;
                    SetDropdownList(ID);
                    return View();
                }
                

                
            }
            
        }
        [HttpPost]
        public ActionResult Index(string ID, Credential entity)
        {
            var credential = new Credential();
            credential.UserGroupID = ID;
            credential.RoleID = entity.UserGroupID;
            var dao = new CredentialDao();

            var res = dao.Insert(credential);
            return RedirectToAction("Index");
        }
        public JsonResult LoadData(string userGroup, int pageIndex, int pageSize)
        {
            var model = new CredentialDao().ListByUserGroup(userGroup);
            int totalRow = model.Count();
            model = model.OrderByDescending(x => x.RoleID)
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
        public JsonResult Delete(string userGroupId, string roleId)
        {
            var res = new CredentialDao().Delete(userGroupId, roleId);
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
        public void SetDropdownList(string userGroupId, string selectedId = null)
        {
            var listRole = new CredentialDao().CheckRoleExist(userGroupId);
            ViewBag.UserGroupID = new SelectList(listRole, "ID", "Name", selectedId);
        }

    }
}