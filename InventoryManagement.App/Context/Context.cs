using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using InventoryManagement.BE.User;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.CompilerServices;

namespace InventoryManagement.App.Context
{
    public class Context
    {

        public static ConcurrentDictionary<Int32, UserContext> _userContextDict = new ConcurrentDictionary<Int32, UserContext>();

        public static UserContext UserContext
        {
            get
            {
                var user = InventoryManagementHttpContext.Current.Session.GetObject<UserContext>("UserContext");

                return user;
            }
        }

        public static void InitiateSession(User user, string token)
        {
            var userContext = new UserContext
            {
                UserInfo = user,
                RolePrivileges = SetPrivileges(user),
                Token = token
            };



            InventoryManagementHttpContext.Current.Session.SetObject("UserContext", userContext.UserInfo.Id);
            _userContextDict.TryAdd(userContext.UserInfo.Id, userContext);

        }

        private static Dictionary<string, bool> SetPrivileges(User user)
        {
            var privileges = user.Privileges.ToDictionary(privilege => privilege.Code, privilege => privilege.Value);

            return privileges;
        }

        public static void TerminateSession()
        {
            if (UserContext != null)
            {
                UserContext u = null;
                _userContextDict.TryRemove(UserContext.UserInfo.Id, out u);
            }
            InventoryManagementHttpContext.Current.Session.SetObject("UserContext", null);
        }
    }

    public static class InventoryManagementHttpContext
    {
        static IServiceProvider services = null;

        /// <summary>
        /// Provides static access to the framework's services provider
        /// </summary>
        public static IServiceProvider Services
        {
            get { return services; }
            set
            {
                if (services != null)
                {
                    throw new Exception("Can't set once a value has already been set.");
                }
                services = value;
            }
        }

        /// <summary>
        /// Provides static access to the current HttpContext
        /// </summary>
        public static HttpContext Current
        {
            get
            {
                IHttpContextAccessor httpContextAccessor = services.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
                return httpContextAccessor?.HttpContext;
            }
        }

    }
}