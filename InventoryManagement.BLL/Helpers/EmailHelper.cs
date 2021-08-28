using InventoryManagement.BE.Email;
using InventoryManagement.Repository;
using InventoryManagement.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InventoryManagement.BLL.Helpers
{
    [DisallowConcurrentExecution]
    public class EmailHelper : IJob
    {
        private readonly IServiceProvider _provider;
        private IEmailRepository _emailRepo;
        private IConfiguration _configuration;
        public EmailHelper(IServiceProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _configuration = configuration;
        }

        public static void SendEmail(string mailAddress, string subject, string mailBody, bool enableSsl = true)
        {
            var avHtml = AlternateView.CreateAlternateViewFromString(mailBody, null, MediaTypeNames.Text.Html);

            using (var mm = new MailMessage())
            {

                mm.To.Add(mailAddress);

                mm.Subject = subject;
                mm.AlternateViews.Add(avHtml);
                mm.From = new MailAddress(Globals.Username);
                mm.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(Globals.HostName))
                {
                    var smtp = new SmtpClient { Host = Globals.HostName };

                    var credential = new NetworkCredential
                    {
                        UserName = Globals.Username,
                        Password = Globals.Password,

                    };

                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = credential;
                    smtp.Port = Globals.Port;
                    smtp.EnableSsl = true;

                    try
                    {
                        smtp.Send(mm);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

        }

        Task IJob.Execute(IJobExecutionContext context)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DBContext>();
            optionsBuilder.UseMySQL(_configuration.GetConnectionString("ConnStr"));

            using (var db = new DBContext(optionsBuilder.Options))
            {
                var pendingEmails = db.Emails.Where(x => x.Status == EmailStatus.Pending).ToList();
                foreach (var email in pendingEmails)
                {
                    try
                    {
                        foreach (var recipient in email.RecipientList)
                        {
                            email.SentDateTime = DateTime.Now;
                            SendEmail(recipient, email.Subject, email.Body);
                        }

                        email.Status = EmailStatus.Sent;
                        db.Entry(email).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        email.FailedReson = Utils.GetExceptionText(ex);
                        email.Status = EmailStatus.Failed;
                        db.Entry(email).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }

            return System.Threading.Tasks.Task.CompletedTask;
        }
    }
    }
