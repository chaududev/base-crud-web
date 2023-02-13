using AppCrud.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppCrud.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
        }
        public IActionResult Index()
        {
            ViewBag.Message = TempData["Message"];
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            //Console.Write(username);
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                TempData["Message"] = "Username not found!!!";
                //  return View();
            }
            else
            {
           var response =  await _signInManager.PasswordSignInAsync(user,password,true, true);
                if (response.Succeeded)
                {
                    return Redirect("/");
                }
                else if(response.IsLockedOut)
                {
                    TempData["Message"] = "Username is lock!!!";
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult Register()
        {
         //   ViewBag.Message = TempData["Message"];
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(string username, string password, string fullname)
        {
            var user = new AppUser()
            {
                UserName = username,
                FullName = fullname,
                EmailConfirmed = true
            };
            var userCurrent =  await _userManager.FindByNameAsync(username);
            if (userCurrent != null)
            {
                ViewData["Message"] = "Username is exist!!!";
                //return View();
            }
            var createResponse = await _userManager.CreateAsync(user, password);
            if (createResponse.Succeeded)
            {
                var role = await _roleManager.FindByNameAsync("USER");
                if (role == null)
                {
                    var roleRegister = new IdentityRole()
                    {
                        Name = "USER",
                        //  NormalizedName="USER"
                    };
                    var responseRole = await _roleManager.CreateAsync(roleRegister);
                    if (responseRole.Succeeded)
                    {
                     var responseAddRoleToUser =   await _userManager.AddToRoleAsync(user, roleRegister.Name);
                        if (responseAddRoleToUser.Succeeded)
                        {
                            ViewData["Message"] = "Register Successs!!!";
                           // return View();
                           
                        }
                        else
                        {
                            ViewData["Message"] = "Can't add role for user!!!";
                         //   return View();
                        }
                       
                      
                    }
                }
                return Redirect("/Auth");
            }

            //  _userManager.AddClaimAsync()

            // _userManager.CreateAsync()
            return View();
        }
    }
}
