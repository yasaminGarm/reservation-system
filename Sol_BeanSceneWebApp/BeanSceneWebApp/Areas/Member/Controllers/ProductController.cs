using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeanSceneWebApp.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BeanSceneWebApp.Areas.Member.Controllers
{
    [Area("Member")]
    public class ProductController : Controller
    {

        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ApplicationDbContext _context;
        public ProductController(IWebHostEnvironment hostEnvironment, ApplicationDbContext context)
        {
            _hostEnvironment = hostEnvironment;
            _context = context;
                
        }

        // GET: Administration/Product
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Administration/Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Administration/Product/Create
        public IActionResult Create()
        {
            ViewData["ProductCategoryId"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Name");
            return View();
        }

        // POST: Administration/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Product product)
        {
            var dbCategory = _context.ProductCategories.Where(pc => pc.Id == product.ProductCategoryId).FirstOrDefault();
            product.Category = dbCategory;

            //if (ModelState.IsValid)
            //{

            //Save image to wwwroot/image
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
            string extension = Path.GetExtension(product.ImageFile.FileName);
            product.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/Images/Product/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(fileStream);
            }

            string pathMobileApp = Path.Combine("C:\\Diploma Project Indivisual\\BeanSceneProject\\BeanSceneMobileApp\\MobileAppUI\\assets\\images\\products\\", fileName);

            using (var fileStream = new FileStream(pathMobileApp, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(fileStream);
            }


            _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            ViewData["ProductCategoryId"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Id", product.ProductCategoryId);
            return View(product);
        }

        // GET: Administration/Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductCategoryId"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Name", product.ProductCategoryId);
            return View(product);
            //I should Store file name then getting the latest file name which were uploded under www/images/product/.. uploaded that from ram/with path for update method ();
            //if user wants to change the image as well should be able to edit the image.means image should seat on the edit page.

          
        }






        // POST: Administration/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.












        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Ingredient,Dietary,ProductCategoryId, ImageName")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            //if (ModelState.IsValid)
            //{
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
           // }
            ViewData["ProductCategoryId"] = new SelectList(_context.Set<ProductCategory>(), "Id", "Id", product.ProductCategoryId);
            return View(product);
        }






        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, Product product)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    // Check if ImageFile is provided, if not, load the existing image
        //    if (product.ImageFile == null)
        //    {
        //        var existingProduct = await _context.Products.FindAsync(id);
        //        product.ImageName = existingProduct.ImageName;
        //    }
        //    else
        //    {
        //        // Handle the case where ImageFile is provided (to update the image).
        //        // You can keep the existing image name or generate a new one, as needed.
        //    }

        //    // Continue with the rest of your Edit action code...

        //    return RedirectToAction(nameof(Index));
        //}


        // GET: Administration/Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

           
       

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Administration/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);


            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images/product", product.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
