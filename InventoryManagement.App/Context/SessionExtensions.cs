using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace InventoryManagement.App.Context
{
    public static class SessionExtensions
    {
        public static void SetObject(this ISession session, string key, object value)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            session.SetString(key, json);
        }

        public static T GetObject<T>(this ISession session, string key) where T : class
        {
            var value = session.GetString(key);
            UserContext userContext = null;

            if (value != null && value != "null")
            {
                switch (key)
                {
                    case "UserContext":
                        Context._userContextDict.TryGetValue(Int32.Parse(value), out userContext);
                        return userContext as T;
                        break;
                    default:
                        return default(T);
                        break;
                }
            }
            else
            {
                return default(T);
            }
        }
    }
}
