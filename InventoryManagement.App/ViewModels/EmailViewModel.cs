using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using InventoryManagement.BE.Email;
using InventoryManagement.BLL.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryManagement.App.ViewModels
{
    public class EmailViewModel 
    {
        public int Id { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailStatus Status { get; set; }
        public int ResendAttempts { get; set; }
        public string FailedReson { get; set; }
        public bool SendInventoryItemStockAllMerchants { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime SentDateTime { get; set; }
        public MerchantViewModel MerchantViewModel { get; set; }
    }
}