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
    [AllowAnonymous]
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

            using (var db = new PortakEntities())
            {
                var user = db.Users.FirstOrDefault(x => x.Username == login.username && x.Password == login.password);

                if(user != null)
                {
                    var x = new
                    {
                        ID = user.Id,
                        Username = user.Username
                    };

                    var json = JsonConvert.SerializeObject(x);
                    FormsAuthentication.SetAuthCookie(json, false);

                    if (Request.QueryString.AllKeys.Contains("ReturnUrl"))
                    {
                        var gidecekadres = Request.QueryString.GetValues("ReturnUrl")[0];
                        if (!string.IsNullOrEmpty(gidecekadres))
                            return Redirect(gidecekadres);


                    }

                    return RedirectToAction("Index", "Home");



                }
                else
                {
                    ViewBag.Message = "Wrong username or password.";
                    
                }

                return View();
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
                        Username = register.Username
                        
                    };

                    var json = JsonConvert.SerializeObject(x);

                    FormsAuthentication.SetAuthCookie(json, false);


                }
            }





                return RedirectToAction("Index","Home");
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            
            return RedirectToAction("Index","Home");
        }

    }
}