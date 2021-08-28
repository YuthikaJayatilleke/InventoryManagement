using AutoMapper;
using InventoryManagement.App.Models;
using InventoryManagement.App.ViewModels;
using InventoryManagement.BE.SecurityPrivilege;
using InventoryManagement.BE.User;
using InventoryManagement.BLL;
using InventoryManagement.BLL.Helpers;
using InventoryManagement.Repository;
using InventoryManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class UserController : Controller// : BaseAuthController
    {
        private IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private DBContext _dBContext;
        public UserController(IUserRepository userRepo, IMapper mapper, DBContext dBContext)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _dBContext = dBContext;
        }
        [AllowAnonymous]
        public IActionResult Edit(int id = 0)
        {
            var userVM = new UserViewModel();
            var user = new User { FirstName = "New", Privileges = new List<SecurityPrivilege>(), Active = true };
            if (id != 0)
            {
                user = _userRepo.GetUserById(id);
            }
            userVM.Id = user.Id;
            userVM.Active = user.Active;
            userVM.Username = user.Username;
            userVM.FirstName = user.FirstName;
            userVM.LastName = user.LastName;
            userVM.IsSuperUser = user.IsSuperUser;
            userVM.Token = Context.Context.UserContext.Token;
            var loadPrivilege = new UserSecurityPrivileges().GetPrivileges(user);
            userVM.Init(loadPrivilege);
            return View(userVM);

        }

        public IActionResult Show(int id)
        {
            var userVM = new UserViewModel();
            var user = _userRepo.GetUserById(id);
            if (user != null)
            {
                userVM.Id = user.Id;
                userVM.Active = user.Active;
                userVM.Username = user.Username;
                userVM.FirstName = user.FirstName;
                userVM.LastName = user.LastName;
                userVM.IsSuperUser = user.IsSuperUser;
                var loadPrivilege = new UserSecurityPrivileges().GetPrivileges(user);
                userVM.Init(loadPrivilege);
            }

            return View(userVM);
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var userVms = new List<UserListViewModel>();
            var users = _userRepo.GetAllUsers();

            foreach (var user in users)
            {
                var userVm = new UserListViewModel
                {
                    Active = user.Active,
                    FirstName = user.FirstName,
                    Id = user.Id,
                    LastName = user.LastName,
                    Username = user.Username
                };

                userVms.Add(userVm);
            }


            return View(userVms);
        }

        public IActionResult Delete(int id)
        {
            _userRepo.Delete(id);
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

                        var vm = JsonConvert.DeserializeObject<UserViewModel>(de);
                        vm.Id = vm.Id < 0 ? 0 : vm.Id;
                        var user = vm.Id == 0 ? new User {Privileges = new List<SecurityPrivilege>()} : _userRepo.GetUserById(vm.Id);
                        user.Privileges.Clear();

                        user.IsSuperUser = vm.IsSuperUser;
                        user.FirstName = vm.FirstName;
                    user.LastName = vm.LastName;
                    user.Username = vm.Username;
                    user.Active = vm.Active;
                    if (vm.IsSetPassword)
                    {
                        if(vm.Password != vm.ConfirmPassword)
                        {
                            return new { Success = false, msg = "Password Missmatch", Type = "Other" };
                        }
                        else
                        {
                            user.Password = Utils.GetEncodedPassword(vm.Password);
                        }
                    }

                    foreach (var privilegeVm in vm.Privileges)
                    {
                        var privilege = new SecurityPrivilege { Code = privilegeVm.Code, User = user, UserId = user.Id, Value = privilegeVm.IsSelected };
                        user.Privileges.Add(privilege);
                    }

                    _userRepo.SaveOrUpdateUser(user, _dBContext);

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
