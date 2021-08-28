using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Data;

namespace InventoryManagement.BLL
{
    public static class Utils
    {
        public static string GetExceptionText(Exception ex)
        {

            if (ex.InnerException == null)
                return ex.Message;

            return ex.Message + "\r\n\r\n+ " + GetExceptionName(ex.InnerException) + GetExceptionText(ex.InnerException);
        }

        private static string GetExceptionName(Exception ex)
        {
            if (ex == null)
                return "";

            return ex.GetType().Name + "\r\n ";
        }


        public static string GetMd5Hash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("X2"));
                }

                // Return the hexadecimal string. 
                return sBuilder.ToString();
            }
        }

        public static string GetEncodedPassword(string password)
        {
            return GetMd5Hash(password + "@YUTHIKA");
        }


    }
}
