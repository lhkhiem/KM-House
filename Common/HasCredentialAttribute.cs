
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var session = (UserLoginSession)HttpContext.Current.Session[ConstantSession.USER_SESSION];
            if (session == null)
            {
                return false;
            }

            List<string> privilegeLevels = this.GetCredentialByLoggedInUser(); // Call another method to get rights of the user from DB

            if (privilegeLevels.Contains(this.RoleID) || session.UserGroupID == Constants.ADMIN_GROUP) // 
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var session = (UserLoginSession)HttpContext.Current.Session[ConstantSession.USER_SESSION];
            if (session == null)
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Login", action = "Index" }));
            }
            else
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Areas/Admin/Views/Shared/401.cshtml"
                };
            }
           
        }
        private List<string> GetCredentialByLoggedInUser()
        {
            var credentials = (List<string>)HttpContext.Current.Session[ConstantSession.SESSION_CREDENTIALS];
            return credentials;
        }
    }
}