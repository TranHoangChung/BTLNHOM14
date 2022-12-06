using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTLNhom14.Data;
using BTLNHOM14.Models;
using BTLNhom14.Models;
using System.Diagnostics;

namespace BTLNhom14.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContex _context;

        public HomeController (ApplicationDbContex context)
        {
            _context = context;
        }

    public IActionResult Index(int? id)
    {
         ProductModel p = new ProductModel();
         if (p==null){
            return View();
         }else
         {
         p.cat=_context.Category.ToList();
         
         if (id==null){
            p.pro=_context.Product.Where(e => e.CategoryID==1).ToList();

         }
         else 
         {
            p.pro=_context.Product.Where(e => e.CategoryID==id).ToList();
         }
         return View(p);
         }
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult ProductCategory(int? id)
    {
         ProductModel p = new ProductModel();
         if (p==null){
            return View();
         }else
         {
         p.cat=_context.Category.ToList();
         
         if (id==null){
            p.pro=_context.Product.Where(e => e.CategoryID==1).ToList();

         }
         else 
         {
            p.pro=_context.Product.Where(e => e.CategoryID==id).ToList();
         }
         return View(p);
         }
    }
    public IActionResult SignIn()
    {
        return View();
    }
    public IActionResult SignUp()
    {
        return View();
    }
    public IActionResult UserDashBoard()
        {
            return View();
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
