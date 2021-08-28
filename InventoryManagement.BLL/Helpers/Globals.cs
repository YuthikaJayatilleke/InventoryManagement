using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace InventoryManagement.BLL.Helpers
{
    public static class Globals
    {
        public static string APIKey { get; set; }
        public static string FromEmail { get; set; }
        public static string Username { get; set; }
        public static string HostName { get; set; }
        public static string Password { get; set; }
        public static int Port { get; set; }
    }
}
