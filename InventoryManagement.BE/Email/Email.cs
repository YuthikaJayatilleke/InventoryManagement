using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace InventoryManagement.BE.Email
{
    public enum EmailStatus
    {
        Pending,
        Sent,
        Failed
    }
    [Table("emails")]
    public class Email : EntityBase
    {
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public EmailStatus Status { get; set; }
        public int ResendAttempts { get; set; }
        public string FailedReson { get; set; }
        public List<string> RecipientList
        {
            get
            {
                var recipients = string.IsNullOrEmpty(Recipient) ? new List<string>() : Recipient.Split(',').ToList();
                return recipients;
            }
        }

        public DateTime CreatedDateTime { get; set; }
        public DateTime SentDateTime { get; set; }
    }

}