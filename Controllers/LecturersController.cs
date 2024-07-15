using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADTest.Data;
using ADTest.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ADTest.Controllers
{
    [Route("Lecturers")]
    public class LecturersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LecturersController> _logger;
        private readonly ApplicationDbContext _context;

        public LecturersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<LecturersController> logger, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "LecturerID")]
            public string LecturerID { get; set; }

            [Required]
            [Display(Name = "LecturerAddress")]
            public string LecturerAddress { get; set; }

            [Required]
            [Display(Name = "ProgramId")]
            public string ProgramId { get; set; }

            [Required]
            [Display(Name = "Fieldofstudy")]
            public string FieldofStudy { get; set; }

            [Required]
            [Display(Name = "Domain")]
            public string Domain { get; set; }

        }
        // GET: Lecturers
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.lecturer.ToListAsync());
        }

        // GET: Lecturers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.lecturer
                .FirstOrDefaultAsync(m => m.LecturerId == id);
            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // GET: Lecturers/Create
        [Route("Create")]
        public IActionResult Create()
        {
            ViewData["ProgramId"] = new SelectList(_context.AcademicProgram, "ProgramId", "ProgramName");
            return View();
        }

        // POST: Lecturers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InputModel Input)
        {
            
            
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    PhoneNumber = Input.PhoneNumber, // Input.Phone should be string
                    CreatedAt = DateTime.Now,
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("New Lecturer account created.");

                    await _userManager.AddToRoleAsync(user, "Lecturer");

                    var lecturer = new Lecturer
                    {
                        LecturerName = user.Name,
                        LecturerId = Input.LecturerID,
                        LecturerAddress = Input.LecturerAddress,
                        ApplicationUserId = user.Id,
                        FieldofStudy = Input.FieldofStudy,
                        domain = Input.Domain,
                        ProgramId = Input.ProgramId
                    };

                    _context.lecturer.Add(lecturer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(Input);
        }

        // GET: Lecturers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.lecturer.FindAsync(id);
            if (lecturer == null)
            {
                return NotFound();
            }
            return View(lecturer);
        }

        // POST: Lecturers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LecturerId,LecturerName,ApplicationUserId,LecturerAddress,ProgramId,FieldofStudy,domain")] Lecturer lecturer)
        {
            if (id != lecturer.LecturerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lecturer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LecturerExists(lecturer.LecturerId))
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
            return View(lecturer);
        }

        // GET: Lecturers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lecturer = await _context.lecturer
                .FirstOrDefaultAsync(m => m.LecturerId == id);
            if (lecturer == null)
            {
                return NotFound();
            }

            return View(lecturer);
        }

        // POST: Lecturers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var lecturer = await _context.lecturer.FindAsync(id);
            if (lecturer != null)
            {
                _context.lecturer.Remove(lecturer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LecturerExists(string id)
        {
            return _context.lecturer.Any(e => e.LecturerId == id);
        }
    }
}
