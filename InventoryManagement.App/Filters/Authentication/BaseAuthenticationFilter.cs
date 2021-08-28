
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using Microsoft.AspNetCore.Http.Headers;
using InventoryManagement.App.Authentication.Microsoft.AspNetCore.Mvc;
using InventoryManagement.App.Context;

namespace InventoryManagement.App.Authentication
{

    public class BaseAuthenticationFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            var routeData = filterContext.RouteData;

            var action = routeData.GetRequiredString("action");
            var controller = routeData.GetRequiredString("controller");



            var method = filterContext.HttpContext.Request.Method;
            var ipAddress = filterContext.HttpContext.Request.Host;



            if (InventoryManagementHttpContext.Current.Session.GetObject<UserContext>("UserContext") == null)
            {
                filterContext.Result = new RedirectResult("~/Login");
                return;
            }

            if (Context.Context.UserContext.UserInfo.IsSuperUser)
                return;

            if ((controller == "Home") && (action == "AuthenticationError" || action == "Index" || action == "SwitchBranch"))
                return;

            if (action.StartsWith("Get") || action.StartsWith("GET")) //Ajax calls
                return;

            if (action == "Index" && Context.Context.UserContext.HasPrivilege(controller + "View"))
                return;

            if (method == "GET" && (action == "Edit" || action == "Create"))
            {
                foreach (string key in filterContext.HttpContext.Request.Query.Keys)
                {
                    if (key.ToUpper() == "ID")
                    {
                        var strVal = filterContext.HttpContext.Request.Query[key];
                        int id;
                        if (int.TryParse(strVal, out id) == false)
                            filterContext.Result = new UnauthorizedResult();

                        if (id == 0)
                        {
                            if (Context.Context.UserContext.HasPrivilege(controller + "Add"))
                                return;

                            SetAuthError(filterContext);
                            return;
                        }

                        if (Context.Context.UserContext.HasPrivilege(controller + "Edit"))
                            return;

                        SetAuthError(filterContext);
                        return;
                    }
                }

                if (Context.Context.UserContext.HasPrivilege(controller + "Add"))
                    return;

                SetAuthError(filterContext);
                return;
            }

            if (action == "Index" && Context.Context.UserContext.HasPrivilege(controller + "List"))
                return;

            if (action == "Show" && Context.Context.UserContext.HasPrivilege(controller + "View"))
                return;

            if (action == "Save" && Context.Context.UserContext.HasPrivilege(controller + "Edit"))
                return;

            if (action == "Save" && Context.Context.UserContext.HasPrivilege(controller + "Add"))
                return;

            if (Context.Context.UserContext.HasPrivilege(controller + action))
                return;

            SetAuthError(filterContext);
        }

        private void SetAuthError(AuthorizationFilterContext filterContext)
        {
            var redirectTargetDictionary = new RouteValueDictionary();
            redirectTargetDictionary.Add("action", "AuthenticationError");
            redirectTargetDictionary.Add("controller", "Home");
            redirectTargetDictionary.Add("error", "You don`t have permission to do this action, Please contact Your Administrator !");
            filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
        }
    }

    public class IgnoreAction : ActionFilterAttribute
    {
        public bool Ignore { get; set; }
    }

    namespace Microsoft.AspNetCore.Mvc
    {
        public static class HelperExtensions
        {
            public static string GetRequiredString(this RouteData routeData, string keyName)
            {
                object value;
                if (!routeData.Values.TryGetValue(keyName, out value))
                {
                    throw new InvalidOperationException($"Could not find key with name '{keyName}'");
                }

                return value?.ToString();
            }
        }
    }
}