using AutoMapper;
using InventoryManagement.App.Models;
using InventoryManagement.App.ViewModels;
using InventoryManagement.BE.Email;
using InventoryManagement.BE.SecurityPrivilege;
using InventoryManagement.BE.User;
using InventoryManagement.BLL;
using InventoryManagement.BLL.Helpers;
using InventoryManagement.BLL.Interfaces;
using InventoryManagement.BLL.Interfaces.BLL.Helpers;
using InventoryManagement.Repository;
using InventoryManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagement.App.Controllers
{
    public class EmailController : BaseAuthController
    {
        private IEmailRepository _emailRepo;
        private IProductRepository _productRepo;
        private readonly IMapper _mapper;
        private IMerchantRepository _merchantRepo;
        private DBContext _dBContext;
        private IMailService _mailService;
        private ICreateEmailHelper _createEmailHelper;
        public EmailController(IEmailRepository emailRepo, IMapper mapper, DBContext dBContext, IProductRepository productRepo, IMerchantRepository merchantRepo, IMailService mailService, ICreateEmailHelper createEmailHelper)
        {
            _emailRepo = emailRepo;
            _mapper = mapper;
            _productRepo = productRepo;
            _dBContext = dBContext;
            _merchantRepo = merchantRepo;
            _mailService = mailService;
            _createEmailHelper = createEmailHelper;
        }

        public IActionResult Edit(int id = 0)
        {
            var emailVM = new EmailViewModel();
            var email = new Email { Subject = "New" };
            if (id != 0)
            {
                email = _emailRepo.GetEmailById(id);
            }
            emailVM.Id = email.Id;
            emailVM.Subject = email.Subject;
            emailVM.Recipient = email.Recipient;
            emailVM.ResendAttempts = email.ResendAttempts;
            emailVM.Status = email.Status;
            emailVM.Body = email.Body;
            emailVM.Status = email.Status;
            emailVM.CreatedDateTime = email.CreatedDateTime;
            emailVM.SentDateTime = email.SentDateTime;
            return View(emailVM);

        }

        public IActionResult Show(int id)
        {
            var emailVM = new EmailViewModel();
            var email = _emailRepo.GetEmailById(id);
            if (email != null)
            {
                emailVM.Id = email.Id;
                emailVM.Subject = email.Subject;
                emailVM.Recipient = email.Recipient;
                emailVM.ResendAttempts = email.ResendAttempts;
                emailVM.Status = email.Status;
                emailVM.Body = email.Body;
                emailVM.Status = email.Status;
                emailVM.CreatedDateTime = email.CreatedDateTime;
                emailVM.SentDateTime = email.SentDateTime;
            }

            return View(emailVM);
        }
        public IActionResult Index()
        {
            var emailVms = new List<EmailListViewModel>();
            var emails = _emailRepo.GetAllEmails();

            foreach (var email in emails)
            {
                var emailVm = new EmailListViewModel
                {
                    Id = email.Id,
                    Subject = email.Subject,
                    Recipient = email.Recipient,
                    ResendAttempts = email.ResendAttempts, 
                    Status = email.Status.ToString(),
                    CreatedDateTime = email.CreatedDateTime,
                    SentDateTime = email.SentDateTime
            };

              emailVms.Add(emailVm);
            }


            return View(emailVms);
        }

        public IActionResult Delete(int id)
        {
            _emailRepo.Delete(id);
            return RedirectToAction("Index");
        }

        public object Save(string data)
        {
                using (var txn = _dBContext.Database.BeginTransaction())
                {
                    try
                    {
                        var base64EncodedBytes = System.Convert.FromBase64String(data.Replace(' ', '+'));
                        var de = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                        var vm = JsonConvert.DeserializeObject<EmailViewModel>(de);
                        vm.Id = vm.Id < 0 ? 0 : vm.Id;
                        var email = vm.Id == 0 ? new Email { CreatedDateTime = System.DateTime.Now , Status  =EmailStatus.Pending } : _emailRepo.GetEmailById(vm.Id);

                    email.Recipient = vm.Recipient;
                    email.Body = vm.Body;
                    email.Subject = vm.Subject;
                    if (vm.SendInventoryItemStockAllMerchants)
                    {
                        sendBulkMail();
                    }
                    else
                    {
                        _emailRepo.SaveOrUpdateEmail(email, _dBContext);
                    }
                    

                    txn.Commit();
                        return new { Success = true, msg = "", Type = "Other" };
                    }
                    catch (Exception ex)
                    {
                        return new { Success = false, msg = ex.Message, Type = "Other" };
                    }
                }
        }

        public object BulkEmail(string data)
        {
            using (var txn = _dBContext.Database.BeginTransaction())
            {
                try
                {
                    var base64EncodedBytes = System.Convert.FromBase64String(data.Replace(' ', '+'));
                    var de = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                    var vm = JsonConvert.DeserializeObject<EmailViewModel>(de);
                    vm.Id = vm.Id < 0 ? 0 : vm.Id;
                    var email = new Email { CreatedDateTime = System.DateTime.Now, Status = EmailStatus.Sent };

                    email.Recipient = vm.Recipient;
                    email.Body = vm.Body;
                    email.Subject = vm.Subject;

                    sendBulkMailUsingSendGrid(email);

                    txn.Commit();
                    return new { Success = true, msg = "", Type = "Other" };
                }
                catch (Exception ex)
                {
                    return new { Success = false, msg = ex.Message, Type = "Other" };
                }
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public void sendBulkMail()
        {
            var body = _createEmailHelper.GetProductStockReport();
            var allMerchants =
                    _merchantRepo.GetAllMerchants().Where(x => !string.IsNullOrEmpty(x.EMail)).ToList();
            string subject = "<h1> Dear!, Inventory Stock at " + DateTime.Now + " </p>";
            var email = new Email { CreatedDateTime = System.DateTime.Now, Status = EmailStatus.Pending, Body = body, Subject = subject };

            foreach (var merchant in allMerchants)
            {
                email.Recipient += merchant.EMail + ",";
            }
            if(!string.IsNullOrEmpty(email.Recipient)){
                email.Recipient = email.Recipient.Remove(email.Recipient.Length - 1);
            }
            _emailRepo.SaveOrUpdateEmail(email, _dBContext);

        }

        public void sendBulkMailUsingSendGrid(Email email)
        {
            var body = _createEmailHelper.GetProductStockReport();
            var allMerchants =
                    _merchantRepo.GetAllMerchants().Where(x => !string.IsNullOrEmpty(x.EMail)).ToList();
            string subject = "<h1>Dear!, Inventory Stock at " + DateTime.Now + "</p>";
            email.Subject += subject;
            email.Body += body;
            var toEmails = new List<string>();
            foreach (var merchant in allMerchants)
            {
                email.Recipient += merchant.EMail + ",";
                toEmails.Add(merchant.EMail);
            }

            var result = _mailService.CreateSingleEmailToMultipleRecipients(toEmails, email.Subject, email.Body);
            if (result.IsSuccessStatusCode)
            {
                _emailRepo.SaveOrUpdateEmail(email, _dBContext);
            }
            else
            {
                throw new Exception("Email Send Fialed !");
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public ActionResult AuthenticationError(string error)
        {
            ViewBag.Error = error;
            return View();
        }
    }
}
