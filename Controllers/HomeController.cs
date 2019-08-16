using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using bankAccount.Models;

namespace bankAccount.Controllers
{
    public class HomeController : Controller
    {
        private BankContext dbContext;
        public HomeController(BankContext context){
            dbContext = context; 
        }

        public IActionResult Index() {
            return View();
        }

        [HttpGet("loginPage")]
        public IActionResult LoginPage() {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(User newUser) {
            if(ModelState.IsValid){
                if(dbContext.users.Any(u => u.Email == newUser.Email)){
                    ModelState.AddModelError("Email", "Email is already in use!");
                    return View("Index");
                } else {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                    dbContext.Add(newUser);
                    dbContext.SaveChanges();
                    if(HttpContext.Session.GetInt32("UserId") == null){
                        HttpContext.Session.SetInt32("UserId", newUser.UserId);
                    }
                    return RedirectToAction("Success", newUser.UserId);
                }
            } else {
                System.Console.WriteLine("*******************");
                System.Console.WriteLine("REGISTRATION NOT WORKING!!!!");
                System.Console.WriteLine("*******************");
                return View("Index");
            }
        }

        public IActionResult Login(LoginUser userLogin){
            if(ModelState.IsValid){
                User userInDB = dbContext.users.FirstOrDefault(u => u.Email == userLogin.Email);
                if(userInDB == null){
                    ModelState.AddModelError("Email", "Invalid email or password");
                    return View("LoginPage");
                } else {
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(userLogin, userInDB.Password, userLogin.Password);
                    if(result == 0){
                        ModelState.AddModelError("Password", "Invalid email or password");
                        return RedirectToAction("LoginPage");
                    }
                    if(HttpContext.Session.GetInt32("UserId") == null){
                        HttpContext.Session.SetInt32("UserId", userInDB.UserId);
                    }
                    return RedirectToAction("Success", new { userID = userInDB.UserId });
                }
            } else {
                return View("LoginPage");
            }
        }

        [HttpGet("success/{userID}")]
        public IActionResult Success(int userID) {
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("LoginPage");
            }
            User currentUser = dbContext.users.Include(u => u.Transactions).FirstOrDefault(u => u.UserId == userID);
            SuccessView myViewModel = new SuccessView();
            myViewModel.User = currentUser; 
            return View("Success", myViewModel);
        }

        [HttpPost("addTrans")]
        public IActionResult addTrans(SuccessView modelData) {
            Transaction newTrans = modelData.Transaction;
            User currentUser = dbContext.users.Include(u => u.Transactions).FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
            if (ModelState.IsValid) {
                if (newTrans.Amount < 0 && Math.Abs(newTrans.Amount) > currentUser.CurrentBalance) {
                    return RedirectToAction("Success", new { userID = currentUser.UserId });
                }
                newTrans.UserId = currentUser.UserId;
                currentUser.CurrentBalance += newTrans.Amount;
                currentUser.UpdatedAt = DateTime.Now;
                dbContext.Add(newTrans);
                dbContext.SaveChanges();
                return RedirectToAction("Success", new { userID = currentUser.UserId });
            }
            SuccessView myViewModel = new SuccessView();
            myViewModel.User = currentUser;
            return View("Success", myViewModel);
        }

        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("LoginPage");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
