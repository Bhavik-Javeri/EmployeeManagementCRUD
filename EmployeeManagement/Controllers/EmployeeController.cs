using EmployeeManagement.Data;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            var employees = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                employees = employees.Where(e => e.Name.Contains(searchTerm) || e.Id.ToString() == searchTerm);
            }

            return PartialView("_EmployeeList", await employees.ToListAsync()); // ✅ Sirf tbody return karega
        }


        public async Task<IActionResult> Filter(string department)
        {
            var employees = _context.Employees.AsQueryable();

            if (!string.IsNullOrEmpty(department))
            {
                employees = employees.Where(e => e.Department == department);
            }

            return PartialView("_EmployeeList", await employees.ToListAsync());
        }

        // 🟢 READ - Get all employees
        public async Task<IActionResult> Index(string searchTerm, string department)
        {
            var employees = _context.Employees.AsQueryable();

            // 🔍 Search by Name or ID
            if (!string.IsNullOrEmpty(searchTerm))
            {
                employees = employees.Where(e => e.Name.Contains(searchTerm) || e.Id.ToString() == searchTerm);
            }

            // 🔹 Filter by Department
            if (!string.IsNullOrEmpty(department))
            {
                employees = employees.Where(e => e.Department == department);
            }

            // 🔽 Pass department list for dropdown
            ViewBag.Departments = await _context.Employees
                                      .Select(e => e.Department)
                                      .Distinct()
                                      .ToListAsync();

            return View(await employees.ToListAsync());
        }


        // 🔵 CREATE - Show Form
        public IActionResult Create()
        {
            return View();
        }

        // 🔵 CREATE - Save to Database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // 🟡 UPDATE - Show Form
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // 🟡 UPDATE - Save Changes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // 🔴 DELETE - Show Confirmation
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // 🔴 DELETE - Remove from Database
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
