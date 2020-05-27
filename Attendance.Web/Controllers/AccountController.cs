using Attendance.Core;
using Attendance.Core.Interface;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Attendance.Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public IAuthenticationManager _auth()
        {
            return HttpContext.GetOwinContext().Authentication;
        }

        private IUserManager _usrmgr;
        public AccountController(IUserManager usrmgr)
        {
            _usrmgr = usrmgr;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (TempData["message"] != null)
            {
                ViewBag.Error = TempData["message"];
                ModelState.AddModelError(string.Empty, ViewBag.Error.ToString());
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _usrmgr.ValidateUser(model);
                if (result == true)
                {
                    List<Claim> claims = new List<Claim>()
                   {
                     new Claim(ClaimTypes.NameIdentifier, model.UserName),
                     new Claim(ClaimTypes.Name, model.UserName),
                   };
                    ClaimsIdentity identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                    _auth().SignIn(identity);
                    return RedirectToAction("Index", "Student");
                }
                ModelState.AddModelError(String.Empty, "This Username is not valid");
                return View(model);
            }
            ModelState.AddModelError(String.Empty, "This Username is not valid");
            return View(model);
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            _auth().SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(UserModel model)
        {
            if (ModelState.IsValid)
            {
                _usrmgr.SignUp(model);
                return RedirectToAction("Login");
                TempData["Message"] = "Enter your User name and password";
            }
            return View(model);
        }
    }
}