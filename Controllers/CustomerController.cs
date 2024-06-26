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
using Microsoft.AspNetCore.Authorization;

namespace EcommerceApp.Controllers
{
    [Authorize]
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
            var user = User; // Get the current user

            // Check if the user is authenticated
            if (user.Identity.IsAuthenticated)
            {
                // Check if the user has the "Customer" role
                if (user.IsInRole("Customer"))
                {
                    _logger.LogInformation("Is in role Customer");
                }
                else
                {
                    _logger.LogInformation("Is NOT in role Customer");
                }
            }


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

        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Customer, Administrator")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Administrators, Customers")]

        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,Address,PhoneNumber,UserId")] Customer customer)
        {
            // Check if the user is authenticated

            _logger.LogInformation("customer Userid: " + customer.UserId);
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

            ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "UserName", customer.UserId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");

            return View(customer);

        }

        // GET: Customer/Edit/5
        [Authorize(Roles = "Administrator, Customer")]
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
        [Authorize(Roles = "Administrator, Customer")]

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
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]

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
