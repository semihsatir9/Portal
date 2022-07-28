using Newtonsoft.Json;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portal.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Login()
        {
            using (var db = new PortakEntities())
            {
                //database erişim

                
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel login)
        {

            using (var db = new PortakEntities())
            {

                

            }
            
            //check database
            //set auth cookie

            return View(login);
        }

        public ActionResult Register()
        { 
            return View(new User());
        }

        [HttpPost]
        public ActionResult Register(User register){

            
            if(string.IsNullOrEmpty(register.Username) || register.Password == null || register.BirthDay == null)
            {
                ViewBag.Message = "There are empty fields.";
                return View(register);
            }


            using (var db = new PortakEntities())
            {
                var isUsernameTaken = db.Users.Any(x => x.Username == register.Username);

                if (isUsernameTaken)
                {
                    ViewBag.Message = "The username is taken.";
                    return View(register);
                }
                else
                {
                    db.Users.Add(register);
                    db.SaveChanges();

                    var x = new
                    {
                        ID = register.Id,
                        Username = register.Username,
                        Password = register.Password
                    };

                    var json = JsonConvert.SerializeObject(x);

                    FormsAuthentication.SetAuthCookie(json, false);


                }
            }





                return RedirectToAction("Index","Home");
        }

        public ActionResult Logout()
        {
            //auth cookie reset
            
            return RedirectToAction("Index","Home");
        }

    }
}