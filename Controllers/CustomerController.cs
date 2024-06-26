using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EcommerceApp.Data;
using EcommerceApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;

namespace EcommerceApp.Controllers
{
    public class CustomerController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerController> _logger;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CustomerController(ApplicationDbContext context, ILogger<CustomerController> logger, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            var customers = await _context.Customers.Include(c => c.User).ToListAsync();

            var customerRoles = new List<CustomerWithRoles>();

            foreach (var customer in customers)
            {
                var roles = await _userManager.GetRolesAsync(customer.User);
                customerRoles.Add(new CustomerWithRoles
                {
                    Customer = customer,
                    Roles = roles
                });
            }

            return View(customerRoles);
        }


        //await _userManager.AddToRoleAsync(user, "Customer");
        //ViewData["Role"] = "Hehe";
        //            _userManager.GetRolesAsync()


        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            //ViewData["UserId"] = new SelectList(_roleManager.Roles, "Id", "Name");
            //ViewData["RoleId"] = new SelectList(_roleManager.Roles, "Id", "Name");

            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Address,PhoneNumber,UserId")] Customer customer)
        {
            _logger.LogInformation("customer Userid: " + customer.UserId);
            //_logger.LogInformation("customer Userid: " + customer.User.Id);
            var user = await _userManager.FindByIdAsync(customer.UserId);


            if (user != null)
            {

                // Assign the ApplicationUser object to the Customer.User property
                customer.User = user;

                // Add the Customer entity to the context
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Handle the case where the ApplicationUser is not found
                ModelState.AddModelError("", "ApplicationUser not found.");
            }

            /*
                        if (ModelState.IsValid)
                        {
                            _logger.LogInformation("customer is valid");

                            _context.Add(customer);
                            await _context.SaveChangesAsync();
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            _logger.LogInformation("customer is invalid");
                        }
                        */

            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", customer.UserId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");

            return View(customer);

        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", customer.UserId);
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,Address,PhoneNumber,UserId")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", customer.UserId);
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
