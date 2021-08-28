using AutoMapper;
using InventoryManagement.App.ViewModels;
using InventoryManagement.BE.SecurityPrivilege;
using InventoryManagement.BE.User;
using InventoryManagement.BLL;
using InventoryManagement.BLL.Interfaces.BLL.Helpers;
using InventoryManagement.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InventoryManagement.App.Controllers
{
    public class LoginController : Controller
    {
        private IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private IJwtAuthenticationManager _jwtAuthenticationManager;

        public LoginController(IUserRepository userRepo, IMapper mapper, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        public ActionResult Index()
        {
            var vm = new LoginViewModel();
            Context.Context.TerminateSession();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel vm)
        {
            if (string.IsNullOrEmpty(vm.UserName))
            {
                @ViewBag.ErrorMsg = "Please Add username";
                return View("Index", vm);
            }

            var user = AuthenticateUser(vm.UserName.Trim());
            if (user.Id == 0)
            {
                @ViewBag.ErrorMsg = "Invalid username";
                return View("Index", vm);
            }

            if (!user.Active)
            {
                @ViewBag.ErrorMsg = "User inactive!";
                return View("Index", vm);
            }


            var encodedPw = Utils.GetEncodedPassword(vm.Password);
            if (user.Password != encodedPw)
            {
                @ViewBag.ErrorMsg = "Invalid password";
                return View("Index", vm);
            }
            var token = _jwtAuthenticationManager.Authenticate(user);
            Context.Context.InitiateSession(user, token);
            return RedirectToAction("Index", "Home");

        }

        private User AuthenticateUser(string username)
        {
            var user = _userRepo.GetUserByUsername(username);
            return user;
        }
    }
}