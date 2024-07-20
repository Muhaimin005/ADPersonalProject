using ADTest.Data;
using ADTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ADTest.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager,
            ILogger<HomeController> logger, 
            ApplicationDbContext context
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            await InitializeAdmin();
            var result = await StudentCheck();
            if (result != null)
            {
                return result;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task InitializeAdmin()
        {
            var adminEmail = "admin@gmail.com";
            var password = "Abc123?";
            var admin = new ApplicationUser
            {
                IC = "admin",
                UserName = adminEmail,
                Email = adminEmail,
                Name = "Admin",
                PhoneNumber = "0111111111",
                CreatedAt = DateTime.Now,
            };
                
            var result = await _userManager.CreateAsync(admin, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        public async Task<IActionResult> StudentCheck()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var roles = _userManager.GetRolesAsync(await _userManager.GetUserAsync(User)).Result;

                if (roles.Contains("Student"))
                {
                    var user = await _userManager.GetUserAsync(User);
                    var userId = user.IC;
                    var student = await _context.student.FirstOrDefaultAsync(s => s.StudentId == userId);

                    if (student == null)
                    {
                        return RedirectToAction("Create", "Students");
                    }
                }
            }

            return null;
        }
    }
}
