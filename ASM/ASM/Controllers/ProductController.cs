using ASM.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
namespace ASM.Controllers
{
    public class ProductController : Controller
    {
        private static BanGiayPs17468Context _context;
        private readonly IEmailSender _emailSender;
        public ProductController(BanGiayPs17468Context context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Role = HttpContext.Session.GetString("role");
            ViewBag.Email = HttpContext.Session.GetString("email");
            var banGiayPs17468Context = _context.Product.Include(p => p.Supplier);
            return View(await banGiayPs17468Context.ToListAsync());
        }
        public IActionResult Login()
        {

            return View();
        }
        public string Email;
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            HttpContext.Session.SetInt32("idCustomer", 0);
            Customer cs = new Customer();
            cs = _context.Customer.Select(p => p)
                .Where(p => p.Email == email && p.PassWord == password).FirstOrDefault();
            if (cs == null)
            {
                ViewBag.login = "Not exist Accouct";
            }
            else
            {
                var Status = _context.Customer.Select(p => p)
                                .Where(p => p.Email == email && p.PassWord == password && p.Status == true).FirstOrDefault();
                if (Status == null)
                {
                    ViewBag.login = "Accouct pause operation";
                }
                else
                {
                    HttpContext.Session.SetString("email", email);
                    var role = _context.Customer.Select(p => p)
                                    .Where(p => p.Email == email && p.PassWord == password && p.Role == 1).FirstOrDefault();
                    int id = _context.Customer.Where(p => p.Email == email).Select(p => p.CustomerId).FirstOrDefault();
                    HttpContext.Session.SetInt32("idCustomer", id);
                    if (role == null)
                    {
                        HttpContext.Session.SetString("role", "0");
                    }
                    else
                    {
                        HttpContext.Session.SetString("role", "1");
                    }
                    Email = email;
                    return RedirectToAction("Index", "Product");
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("email");
            HttpContext.Session.Remove("idCustomer");
            HttpContext.Session.Remove("role");
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> DetailProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        public IActionResult DetailCustomer()
        {
            var detail = _context.CustomerDetail.Select(p => p).Where(p => p.CustomerId == HttpContext.Session.GetInt32("idCustomer")).FirstOrDefault();
            return View(detail);
        }
        [HttpPost]
        public IActionResult DetailCustomer(string name, int age, string deliveryAddress, string phone)
        {
            ViewBag.Error = "";
            CustomerDetail cs = new CustomerDetail();

            if(name == null || age < 0 || deliveryAddress == null || phone == null)
            {
                ViewBag.Error = "Invalid Info";
            }
            else
            {
                cs = _context.CustomerDetail.Select(p => p).Where(p => p.CustomerId == HttpContext.Session.GetInt32("idCustomer")).FirstOrDefault();
                cs.Name = name;
                cs.Age = age;
                cs.DeliveryAddress = deliveryAddress;
                cs.Phone = phone;
                _context.CustomerDetail.Update(cs);
                _context.SaveChanges();
                ViewBag.Error = "Update Success";
            }

            return View(cs);
        }

        public IActionResult Help()
        {
            return View();
        }

        public const string CARTKEY = "cart";
        List<ItemCart> GetCartItem()
        {
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if(jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<ItemCart>>(jsoncart);
            }
            return new List<ItemCart>();
        }

        // remove cart for session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }
        // Save List cart on session
        void SaveCartSession(List<ItemCart> ls)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        //Create cart
        //Add product for cart
        [Route("addcart/{productid:int}", Name = "addcart")]
        public IActionResult AddToCart([FromRoute]int productid)
        {
            var product = _context.Product.Where(p => p.ProductId == productid)
                                            .FirstOrDefault();
            if(product == null)
            {
                return NotFound("Cart have not product");
            }
            //add cart
            var cart = GetCartItem();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if(cartitem != null)
            {
                cartitem.quantity++;
            }
            else
            {
                cart.Add(new ItemCart()
                {
                    quantity = 1,
                    product = product
                });
            }
            //save on session
            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }
        // display cart
        [Route("/cart", Name = "cart")]
        public IActionResult Cart()
        {
            return View(GetCartItem());
        }

        //Update cart
        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart(int SanPhamID, int soluongmoi)
        {
            var cart = GetCartItem();
            var cartitem = cart.FirstOrDefault(p => p.product.ProductId == SanPhamID);
            if (cartitem != null)
            {
                cartitem.quantity = soluongmoi;
            }
            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }



        //remove product in cart
        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = GetCartItem();
            var cartitem = cart.Find(p => p.product.ProductId == productid);
            if (cartitem != null)
            {
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Cart));
        }

        [HttpPost]
        public IActionResult Pay(decimal Sumprice,string name, string phone, string address)
        {
            ViewBag.Error = "";
            if (address == null || phone == null || name == null)
            {
                ViewBag.Error = "Missing consignee information";
                //return RedirectToAction(nameof(Cart));
            }
            else
            {
                var cart = GetCartItem();
                string hour = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Cart cr = new Cart();
                cr.CustomerId = (int)HttpContext.Session.GetInt32("idCustomer");
                cr.CreateDate = DateTime.Parse(hour);
                cr.Status = false;
                cr.SumPrice = Sumprice;
                cr.Phone = phone;
                cr.Address = address;
                cr.NameRecipient = name;
                _context.Cart.Add(cr);
                _context.SaveChanges();
                foreach (var item in cart)
                {
                    int id = _context.Cart.Where(p => p.CreateDate == DateTime.Parse(hour)).Select(p => p.CartId).FirstOrDefault();
                    CartDetail dt = new CartDetail();
                    dt.CartId = id;
                    dt.ProductId = item.product.ProductId;
                    dt.Quantily = item.quantity;
                    decimal total = item.quantity * item.product.Price;
                    dt.Total = total;
                    _context.CartDetail.Add(dt);
                                       
                    _context.SaveChanges();
                    int quantityLast = _context.Product.Where(p => p.ProductId == item.product.ProductId)
                                                        .Select(p => p.Quantity).FirstOrDefault();
                    var pr = _context.Product.Select(p => p).Where(p => p.ProductId == item.product.ProductId).FirstOrDefault();
                    pr.Quantity = quantityLast - item.quantity;
                    _context.Product.Update(pr);
                    _context.SaveChanges();

                }
                ClearCart();
                return RedirectToAction(nameof(History));
            }
            return RedirectToAction(nameof(History));
        }
        public IActionResult ForgotPass()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassAsync(string email)
        {

            string pass = "a";
            var cs = _context.Customer.Select(p => p).Where(p => p.Email == email).FirstOrDefault();
            cs.PassWord = pass;
            _context.Customer.Update(cs);
            _context.SaveChanges();

            await _emailSender.SendEmailAsync(email,
                        "Xác nhận địa chỉ email",
                        "Password new: "+ pass);

            //MailMessage msg = new MailMessage();

            //msg.From = new MailAddress("tieptvps17468@fpt.edu.vn");
            //msg.To.Add(email);
            //msg.Subject = "test";
            //msg.Body = "Test Content"+pass;
            ////msg.Priority = MailPriority.High;

            //SmtpClient client = new SmtpClient();

            //client.Credentials = new NetworkCredential("tieptvps17468@fpt.edu.vn", "@iuenhiulam123", "smtp.gmail.com");
            //client.Host = "smtp.gmail.com";
            //client.Port = 587;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.EnableSsl = true;
            //client.UseDefaultCredentials = true;

            //client.Send(msg);
            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("Admin", "tieptvps17468@fpt.edu.vn"));
            //message.To.Add(new MailboxAddress("", email));
            //message.Subject = "Admin send password";
            //message.Body = new TextPart("plain")
            //{
            //    Text = pass
            //};
            //var client = new SmtpClient();
            //client.Host = "smtp.gmail.com";
            //    client.Port = 25;
            //    NetworkCredential cred = new NetworkCredential("tieptvps17468@fpt.edu.vn", "@iuenhiulam123");
            //    client.Send(message);
            //    client.Disconnect(true);
            //client.Send(message);
            //SmtpClient client = new SmtpClient("smtp.gmail.com", 25);
            //NetworkCredential cred = new NetworkCredential("tieptvps17468@fpt.edu.vn", "@iuenhiulam123");
            //MailMessage Msg = new MailMessage();
            //Msg.From = new MailAddress("tieptvps17468@fpt.edu.vn");
            //Msg.To.Add(email);
            //Msg.Subject = "Bạn đã sử dụng tính năng quên mật khẩu";
            //Msg.Body = "Chào anh/chị. Mật khẩu mới là: " + pass;
            //client.Credentials = cred;
            //client.EnableSsl = true;
            //client.Send(Msg);

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> History()
        {
            var banGiayPs17468Context = _context.Cart.Where(p => p.CustomerId == HttpContext.Session.GetInt32("idCustomer")).Include(c => c.Customer);
            return View(await banGiayPs17468Context.ToListAsync());
        }
        public async Task<IActionResult> DetailHitory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.CartId == id);
            string name = _context.CustomerDetail.Where(p => p.CustomerId == cart.CustomerId).Select(p => p.Name).FirstOrDefault();
            ViewBag.Day = cart.CreateDate;
            ViewBag.Status = cart.Status;
            ViewBag.SumPrice = cart.SumPrice;
            ViewBag.Customer = name;
            ViewBag.Recipient = cart.NameRecipient;
            ViewBag.address = cart.Address;
            ViewBag.Phone = cart.Phone;
            ViewBag.id = cart.CartId;
            if (cart == null)
            {
                return NotFound();
            }
            var cartDetail = _context.CartDetail.Where(p => p.CartId == id).Include(p => p.Product).Include(p => p.Cart);
            return View(cartDetail);
        }
        [HttpPost]
        public IActionResult UpdateProductOrder(int id, int idProduct, int quantity)
        {
            int quantityLast = (int)_context.CartDetail.Where(p => p.Stt == id).Select(p => p.Quantily).FirstOrDefault();
            int priceLast = (int)_context.CartDetail.Where(p => p.Stt == id).Select(p => p.Total).FirstOrDefault();
            int quantityProductLast = (int)_context.Product.Where(p => p.ProductId == idProduct).Select(p => p.Quantity).FirstOrDefault();
            int surplus = quantity - quantityLast;
            CartDetail cdt = new CartDetail();
            cdt = _context.CartDetail.Select(p => p).Where(p => p.Stt == id).FirstOrDefault();
            Product pr = new Product();
            pr = _context.Product.Select(p => p).Where(p => p.ProductId == idProduct).FirstOrDefault();
            int cartid = (int)_context.CartDetail.Where(p => p.Stt == id).Select(p => p.CartId).FirstOrDefault();
            Cart cart = new Cart();
            cart = _context.Cart.Select(p => p).Where(p => p.CartId == cartid).FirstOrDefault();
            if (surplus > 0)
            {
                cdt.Quantily = quantity;
                cdt.Total = pr.Price * quantity;
                _context.CartDetail.Update(cdt);
                _context.SaveChanges();

                pr.Quantity = quantityProductLast - surplus;
                _context.Product.Update(pr);
                _context.SaveChanges();

                cart.SumPrice = cart.SumPrice + (cdt.Total - priceLast);
                _context.Cart.Update(cart);
                _context.SaveChanges();
                return RedirectToAction(nameof(History));
            }
            if(surplus < 0)
            {
                cdt.Quantily = quantity;
                cdt.Total = pr.Price * quantity;
                _context.CartDetail.Update(cdt);
                _context.SaveChanges();

                pr.Quantity = quantityProductLast + (quantityLast - quantity);
                _context.Product.Update(pr);
                _context.SaveChanges();

                cart.SumPrice = cart.SumPrice - (priceLast - cdt.Total);
                _context.Cart.Update(cart);
                _context.SaveChanges();
                return RedirectToAction(nameof(History));
            }
            return RedirectToAction(nameof(History));
        }
        [HttpPost]
        public IActionResult RemoveProductOrder(int id, int idProduct, int quantity)
        {
            CartDetail cdt = new CartDetail();
            cdt = _context.CartDetail.Where(p => p.Stt == id).Select(p => p).FirstOrDefault();
            int cartid = (int)_context.CartDetail.Where(p => p.Stt == id).Select(p => p.CartId).FirstOrDefault();
            int priceCDT = (int)_context.CartDetail.Where(p => p.Stt == id).Select(p => p.Total).FirstOrDefault();
            Cart cart = new Cart();
            cart = _context.Cart.Where(p => p.CartId == cartid).Select(p => p).FirstOrDefault();
            int sumprice = (int)_context.Cart.Where(p => p.CartId == cartid).Select(p => p.SumPrice).FirstOrDefault();

            Product pr = new Product();
            pr = _context.Product.Select(p => p).Where(p => p.ProductId == idProduct).FirstOrDefault();
            int quantityPr = (int)_context.Product.Where(p => p.ProductId == idProduct).Select(p => p.Quantity).FirstOrDefault();
            _context.CartDetail.Remove(cdt);
            _context.SaveChanges();

            pr.Quantity = quantityPr + quantity;
            _context.Product.Update(pr);
            _context.SaveChanges();

            if(cart.SumPrice - priceCDT == 0)
            {
                _context.Cart.Remove(cart);
                _context.SaveChanges();
            }
            if(cart.SumPrice - priceCDT > 0)
            {
                cart.SumPrice = sumprice - priceCDT;
                _context.Cart.Update(cart);
                _context.SaveChanges();
            }
            if (cart.SumPrice - priceCDT < 0)
            {
                _context.Cart.Remove(cart);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(History));
        }
        [HttpPost]
        public IActionResult UpdateInfo(int id,string recipient, string phone, string address)
        {
            Cart cart = new Cart();
            cart = _context.Cart.Select(p => p).Where(p => p.CartId == id).FirstOrDefault();

            cart.NameRecipient = recipient;
            cart.Phone = phone;
            cart.Address = address;
            _context.Cart.Update(cart);
            _context.SaveChanges();
            return RedirectToAction(nameof(History));
        }
    }
}
