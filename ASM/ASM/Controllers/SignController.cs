using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM.Controllers
{
    public class SignController : Controller
    {
        public readonly BanGiayPs17468Context _context;
        public SignController(BanGiayPs17468Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public string Role = "";
        public string Email = "";
        
        [HttpPost]
        public IActionResult Index(string email, string password)
        {

            ViewBag.login = "";
            ViewBag.Role = "";
            Customer cs = new Customer();
            cs = _context.Customer.Select(p => p)
                .Where(p => p.Email == email && p.PassWord == password).FirstOrDefault();
            if (cs == null)
            {
                ViewBag.login = "Not exist Accouct";
            }
            else
            {
                ViewBag.Role = _context.Customer.Select(p => p)
                                .Where(p => p.Email == email && p.PassWord == password && p.Role == 1).FirstOrDefault();
                var Status = _context.Customer.Select(p => p)
                                .Where(p => p.Email == email && p.PassWord == password && p.Status == true).FirstOrDefault();
                if (Status == null)
                {
                    ViewBag.login = "Accouct pause operation";
                }
                else
                {
                    return RedirectToAction("Index", "Product");
                }
            }
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(string email, string password)
        {
            ViewBag.kt = "";
            var checkMail = _context.Customer.Select(p => p)
                                .Where(p => p.Email == email).FirstOrDefault();
            Customer cs = new Customer();
            if (checkMail == null)
            {
                try
                {
                    cs.Email = email;
                    cs.PassWord = password;
                    cs.Status = true;
                    cs.Role = 0;
                    _context.Customer.Add(cs);
                    _context.SaveChanges();
                    int id = _context.Customer.Where(p => p.Email == email).Select(p => p.CustomerId).FirstOrDefault();
                    CustomerDetail dt = new CustomerDetail();
                    dt.CustomerId = id;
                    dt.Name = "Not";
                    dt.Age = 0;
                    dt.DeliveryAddress = "Not";
                    dt.Phone = "Not";
                    _context.CustomerDetail.Add(dt);
                    _context.SaveChanges();
                    ViewBag.kt = "sign Up Success";
                }
                catch (Exception ex)
                {
                    ViewBag.kt = "Fail";
                }
            }
            else
            {
                ViewBag.kt = "Email exist";
            }
            return View();
        }
    }
}
