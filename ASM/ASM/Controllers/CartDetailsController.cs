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
    public class CartDetailsController : Controller
    {
        private readonly BanGiayPs17468Context _context;

        public CartDetailsController(BanGiayPs17468Context context)
        {
            _context = context;
        }

        // GET: CartDetails
        public async Task<IActionResult> Index()
        {
            var banGiayPs17468Context = _context.CartDetail.Include(c => c.Cart).Include(c => c.Product);
            return View(await banGiayPs17468Context.ToListAsync());
        }

        // GET: CartDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartDetail = await _context.CartDetail
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Stt == id);
            if (cartDetail == null)
            {
                return NotFound();
            }

            return View(cartDetail);
        }

        // GET: CartDetails/Create
        public IActionResult Create()
        {
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId");
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Images");
            return View();
        }

        // POST: CartDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Stt,CartId,ProductId,Quantily,Total")] CartDetail cartDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", cartDetail.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Images", cartDetail.ProductId);
            return View(cartDetail);
        }

        // GET: CartDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartDetail = await _context.CartDetail.FindAsync(id);
            if (cartDetail == null)
            {
                return NotFound();
            }
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", cartDetail.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Images", cartDetail.ProductId);
            return View(cartDetail);
        }

        // POST: CartDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Stt,CartId,ProductId,Quantily,Total")] CartDetail cartDetail)
        {
            if (id != cartDetail.Stt)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartDetailExists(cartDetail.Stt))
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
            ViewData["CartId"] = new SelectList(_context.Cart, "CartId", "CartId", cartDetail.CartId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Images", cartDetail.ProductId);
            return View(cartDetail);
        }

        // GET: CartDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cartDetail = await _context.CartDetail
                .Include(c => c.Cart)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Stt == id);
            if (cartDetail == null)
            {
                return NotFound();
            }

            return View(cartDetail);
        }

        // POST: CartDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cartDetail = await _context.CartDetail.FindAsync(id);
            _context.CartDetail.Remove(cartDetail);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartDetailExists(int id)
        {
            return _context.CartDetail.Any(e => e.Stt == id);
        }
    }
}
