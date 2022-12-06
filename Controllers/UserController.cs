using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Specialized;
using Microsoft.AspNetCore.Session;
using BTLNhom14.Data;
using BTLNHOM14.Models;

namespace BTLNhom14.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContex _context;

        public UserController (ApplicationDbContex context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.User'  is null.");
        }
        
        public IActionResult SignIn(User acc)
        {
            
            //Session["user"]="";
            var data=_context.User.Where(x =>x.UserName==acc.UserName && x.Password==acc.Password).Count();
            if (data != 0)
            {
                //Session["user"]=acc.UserName;
                return RedirectToAction("AdminDashBoard","User");
            }
            else
            {
                
                return View();
            }
            
        }
        public IActionResult UserDashBoard()
        {
            return View();
        }
        public IActionResult AdminDashBoard()
        {
            return View();
        }


        public IActionResult SignUp()
        {
            return View();
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var User = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (User == null)
            {
                return NotFound();
            }

            return View(User);
        }
        
        // GET: User/Create
        public IActionResult Create(int Id=0)
        {
            
            if (Id == 0)
                return View(new User());
            else
                return View(_context.User.Find(Id));
        }
        

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,FullName,UserName,Password,Age,Phone,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("UserID,FullName,UserName,Password,Age,Phone,Address")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'ApplicationDbContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int? id)
        {
          return (_context.User?.Any(e => e.UserID == id)).GetValueOrDefault();
        }
    }
}