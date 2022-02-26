using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Models;

namespace ASM.Controllers
{
    public class CartsController : Controller
    {
        private readonly BanGiayPs17468Context _context;

        public CartsController(BanGiayPs17468Context context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            var banGiayPs17468Context = _context.Cart.Include(c => c.Customer);
            return View(await banGiayPs17468Context.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public IActionResult Checked(int id)
        {
            var cart = _context.Cart.Where(p => p.CartId == id).Select(p => p).FirstOrDefault();
            cart.Status = true;
            _context.Cart.Update(cart);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Email");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,CustomerId,CreateDate,Status,SumPrice")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Email", cart.CustomerId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Email", cart.CustomerId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,CustomerId,CreateDate,Status,SumPrice")] Cart cart)
        {
            if (id != cart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.CartId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Email", cart.CustomerId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Cart.FindAsync(id);
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.CartId == id);
        }
    }
}
