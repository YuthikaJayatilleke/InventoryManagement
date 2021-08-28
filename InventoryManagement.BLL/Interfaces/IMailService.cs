using SendGrid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BLL.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string toEmail, string subject, string content);
        Response CreateSingleEmailToMultipleRecipients(List<string> toEmail, string subject, string content);
    }
}
