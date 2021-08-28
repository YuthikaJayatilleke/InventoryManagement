using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.App.ViewModels
{
    public class EmailListViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
        public int ResendAttempts { get; set; }
        public string FailedReson { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime SentDateTime { get; set; }
        public override void InitMapping()
        {
            throw new NotImplementedException();
        }
    }
}