using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace ASM.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BanGiayPs17468Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(BanGiayPs17468Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var banGiayPs17468Context = _context.Product.Where(p => p.Status == true).Include(p => p.Supplier);
            return View(await banGiayPs17468Context.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price,Introduce,SupplierId,Status,Images,Quantity")] ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                string stringFileName = UploadFile(model);
                var product = new Product
                {
                    Name = model.Name,
                    Price = model.Price,
                    Introduce = model.Introduce,
                    SupplierId = model.SupplierId,
                    Status = model.Status,
                    Images = stringFileName,
                    Quantity = model.Quantity
                };
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "Name", model.SupplierId);
            return View(model);
        }

        private string UploadFile(ProductViewModel model)
        {
            string uniqueFileName = null;

            if (model.Images != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "product_imgs");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Images.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Images.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var model = new ProductViewModel
            {
                Name = product.Name,
                Price = product.Price,
                Introduce = product.Introduce,
                SupplierId = product.SupplierId,
                Status = product.Status,
                Quantity = product.Quantity,
                ExistingImage = product.Images,
            };
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "Name", product.SupplierId);
            return View(model);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price,Introduce,SupplierId,Status,Quantity,Images")] ProductViewModel model)
        {
            //if (id != model.ProductId)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                var product = await _context.Product.FindAsync(id);

                product.Name = model.Name;
                product.Price = model.Price;
                product.Introduce = model.Introduce;
                product.SupplierId = model.SupplierId;
                product.Status = model.Status;
                product.Quantity = model.Quantity;

                if (model.Images != null)
                {
                    if (model.ExistingImage != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "product_imgs", model.ExistingImage);
                        System.IO.File.Delete(filePath);
                    }
                    product.Images = UploadFile(model);
                }

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
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
            ViewData["SupplierId"] = new SelectList(_context.Supplier, "SupplierId", "Name", model.SupplierId);
            return View(model);
        }

        private string ProcessUploadedFile(ProductViewModel model)
        {
            string uniqueFileName = null;

            if (model.Images != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Images.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Images.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            product.Status = false;
            _context.Product.Update(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
