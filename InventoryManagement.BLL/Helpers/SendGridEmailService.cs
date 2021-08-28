using InventoryManagement.BLL.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.BLL.Helpers
{
    public class SendGridEmailService : IMailService
    {
        public async Task SendEmailAsync(string toEmail, string subject, string content)
        {
            var apiKey = Globals.APIKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("yuthika@evisionmicro.com");
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, content, content);
            var response = await client.SendEmailAsync(msg);
        }

        public Response CreateSingleEmailToMultipleRecipients(List<string> toEmails, string subject, string content)
        {
            var apiKey = Globals.APIKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("yuthika@evisionmicro.com");
            var toEmailsAddress = new List<EmailAddress>();
            foreach (var toEmail in toEmails)
            {
                var to = new EmailAddress(toEmail);
                toEmailsAddress.Add(to);
            }

            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, toEmailsAddress, subject, content, content);
            var response =  client.SendEmailAsync(msg).GetAwaiter().GetResult();
            return response;
        }
    }
}
