using AutoMapper;
using InventoryManagement.App.Models;
using InventoryManagement.App.ViewModels;
using InventoryManagement.BE.Merchant;
using InventoryManagement.BE.Product;
using InventoryManagement.BE.SecurityPrivilege;
using InventoryManagement.BE.User;
using InventoryManagement.BLL;
using InventoryManagement.BLL.Helpers;
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
    public class MerchantController : BaseAuthController
    {
        private IMerchantRepository _merchantRepo;
        private readonly IMapper _mapper;
        private DBContext _dBContext;
        public MerchantController(IMerchantRepository merchantRepo, IMapper mapper, DBContext dBContext)
        {
            _merchantRepo = merchantRepo;
            _mapper = mapper;
            _dBContext = dBContext;
        }

        public IActionResult Edit(int id = 0)
        {
            var merchantVM = new MerchantViewModel();
            var merchant = new Merchant { Code = "New"};
            if (id != 0)
            {
                merchant = _merchantRepo.GetMerchantById(id);
            }
            merchantVM = _mapper.Map(merchant, new MerchantViewModel());
            return View(merchantVM);

        }

        public IActionResult Show(int id)
        {
            var merchantVM = new MerchantViewModel();
            var merchant = _merchantRepo.GetMerchantById(id);
            if (merchant != null)
            {
                merchantVM = _mapper.Map(merchant, new MerchantViewModel());
            }

            return View(merchantVM);
        }
        public IActionResult Index()
        {
            var merchantVms = new List<MerchantListViewModel>();
            var merchants = _merchantRepo.GetAllMerchants();
            
            foreach (var merchant in merchants)
            {
               var merchantVM = new MerchantListViewModel
               {
                    Code = merchant.Code,
                    Name = merchant.Name,
                    Id = merchant.Id
               };
                merchantVms.Add(merchantVM);
            }


            return View(merchantVms);
        }

        public IActionResult Delete(int id)
        {
            _merchantRepo.Delete(id);
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

                        var vm = JsonConvert.DeserializeObject<MerchantViewModel>(de);
                        vm.Id = vm.Id < 0 ? 0 : vm.Id;
                        var merchant = vm.Id == 0 ? new Merchant() : _merchantRepo.GetMerchantById(vm.Id);
                       _mapper.Map(vm, merchant);
                    _merchantRepo.SaveOrUpdateMerchant(merchant, _dBContext);

                    txn.Commit();
                        return new { Success = true, msg = "", Type = "Other" };
                    }
                    catch (Exception ex)
                    {
                        return new { Success = false, msg = ex.Message, Type = "Other" };
                    }
                }
        }

        public ActionResult GetMerchantList()
        {
            var merchants = new List<Merchant>();
            var merchant = new Merchant { Id = 0 , Code = "Select Merchant" };
            merchants.Add(merchant);
            var allMerchants =
                     _merchantRepo.GetAllMerchants().Where(x => !string.IsNullOrEmpty(x.EMail)).ToList();
            merchants.AddRange(allMerchants);

            var list = merchants.Select(x => new
            {
                Id = x.Id,
                Code = x.Code,
                EMail = x.EMail
            }).ToList();

            return Json(list);
        }

        public IActionResult Privacy()
        {
            return View();
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
