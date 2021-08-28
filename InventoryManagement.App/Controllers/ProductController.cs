using AutoMapper;
using InventoryManagement.App.Models;
using InventoryManagement.App.ViewModels;
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
    public class ProductController : BaseAuthController
    {
        private IProductRepository _produtRepo;
        private readonly IMapper _mapper;
        private DBContext _dBContext;
        public ProductController(IProductRepository produtRepo, IMapper mapper, DBContext dBContext)
        {
            _produtRepo = produtRepo;
            _mapper = mapper;
            _dBContext = dBContext;
        }

        public IActionResult Edit(int id = 0)
        {
            var productVM = new ProductViewModel();
            var product = new Product { Code = "New"};
            if (id != 0)
            {
                product = _produtRepo.GetProductById(id);
            }
            productVM = _mapper.Map(product, new ProductViewModel());
            return View(productVM);

        }

        public IActionResult Show(int id)
        {
            var productVM = new ProductViewModel();
            var product = _produtRepo.GetProductById(id);
            if (product != null)
            {
                productVM = _mapper.Map(product, new ProductViewModel());
            }

            return View(productVM);
        }
        public IActionResult Index()
        {
            var productVms = new List<ProductListViewModel>();
            var products = _produtRepo.GetAllProducts();
            
            foreach (var product in products)
            {
               var productVM = new ProductListViewModel
               {
                    Code = product.Code,
                    CurrentQty = product.CurrentQty,
                    Name = product.Name,
                    Id = product.Id
               };
                productVms.Add(productVM);
            }


            return View(productVms);
        }

        public IActionResult Delete(int id)
        {
            _produtRepo.Delete(id);
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

                        var vm = JsonConvert.DeserializeObject<ProductViewModel>(de);
                        vm.Id = vm.Id < 0 ? 0 : vm.Id;
                        var product = vm.Id == 0 ? new Product() : _produtRepo.GetProductById(vm.Id);
                       _mapper.Map(vm, product);
                       _produtRepo.SaveOrUpdateProduct(product, _dBContext);

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
