using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTLNhom14.Data;
using BTLNhom14.Models.Process;
using BTLNHOM14.Models;

namespace BTLNhom14.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContex _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private ExcelProcess _excelProcess = new ExcelProcess();
        private StringProcess strPro = new StringProcess();
        public ProductController(ApplicationDbContex contex, IWebHostEnvironment hostEnvironment)
        {
            _context = contex;
            this._hostEnvironment = hostEnvironment;
        }
   // GET: Product
        public async Task<IActionResult> Index()
        {

            return View(await _context.Product.ToListAsync());
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        public IActionResult AddOrEdit(int Id=0)
        {
            ViewData["CategoryID"]=new SelectList(_context.Category, "CategoryID", "CategoryName");

            if (Id == 0)
                return View(new Product());
            else
                return View(_context.Product.Find(Id));
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("ProductID,ProductName,Info,CategoryID,Price,Sale,Note,ImageFile")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.ProductID != 0){

                    //Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    product.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }   
                    _context.Add(product);
                }
                else
                {
                    _context.Update(product);
                  
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"]=new SelectList(_context.Category, "CategoryID", "CategoryName",product.CategoryID);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["CategoryID"]=new SelectList(_context.Category, "CategoryID", "CategoryName");
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,Price,Info,Sale,Note,ImageName,ImageFile,CategoryID")] Product product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    product.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }   
                    _context.Add(product);   
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductID))
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
            ViewData["CategoryID"]=new SelectList(_context.Category, "CategoryID", "CategoryName",product.CategoryID);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ApplicationDbContex.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Upload")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file != null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    Console.WriteLine("cant upload");
                    ModelState.AddModelError("Product", "Please choose excel file to upload!");
                }
                else
                {
                    //rename file when upload to server 
                    var fileName = DateTime.Now.ToShortTimeString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads_Excels", fileName);
                    var fileLocation = new FileInfo(filePath).ToString();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        //save file to update
                        await file.CopyToAsync(stream);
                        var dt = _excelProcess.ExcelToDataTable(fileLocation);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            var pro = new Product();
                            //pro.ProductID = Convert.ToInt32(dt.Rows[i][0].ToString());
                            pro.ProductName = dt.Rows[i][1].ToString();
                            pro.CategoryID = Convert.ToInt32(dt.Rows[i][2].ToString());
                            pro.Info = dt.Rows[i][3].ToString();
                            pro.Sale = Convert.ToInt32(dt.Rows[i][4].ToString());
                            pro.Price = Convert.ToInt32(dt.Rows[i][5].ToString());
                            pro.Note = dt.Rows[i][6].ToString();
                            pro.ImageName=dt.Rows[i][7].ToString();
                            _context.Product.Add(pro);
                        }
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            else
            {
                Console.WriteLine("cant upload");
            }
            return View();
        }

        private bool ProductExists(int? id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }
    }
}
