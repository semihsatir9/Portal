using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {
            //check database
            //set auth cookie

            return View(login);
        }

        public ActionResult Register()
        { 
            return View();
        }

        [HttpPost]
        public ActionResult Register(LoginModel register){

            //database model to register
            //set auth cookie

            return View(register);
        }

        public ActionResult Logout()
        {
            //auth cookie reset
            
            return RedirectToAction("Index","Home");
        }

    }
}