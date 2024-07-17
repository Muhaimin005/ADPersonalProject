using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using ADTest.Data;
using ADTest.Models;
using ADTest.Models.ViewModel;
using ADTest.Migrations;

namespace ADTest.Controllers
{
    public class CommitteeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<CommitteeController> _logger;
        private readonly ApplicationDbContext _context;

        public CommitteeController
        (
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            ILogger<CommitteeController> logger, 
            ApplicationDbContext context
        )
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
            [Display(Name = "CommitteeId")]
            public string CommitteeId { get; set; }

            [Required]
            [Display(Name = "ProgramId")]
            public string ProgramId { get; set; }
        }

        public async Task<IActionResult>AssignDomain()
        {
            List<Lecturer> lecturerList = new List<Lecturer>();
            return View(lecturerList);
        }
		public IActionResult UpdateDomain(string status, string lecturerId)
		{
			// Retrieve the billing record based on billingId
			var lecturer = _context.lecturer.FirstOrDefault(l => l.LecturerId == lecturerId);
			if (lecturer != null)
			{
				lecturer.domain = status;
				_context.SaveChanges();
				// Handle success (return JSON or redirect)
				return Json(new { success = true });
			}
			// Handle error (return JSON or appropriate response)
			return Json(new { success = false });
		}

        [Route("Create")]
        public IActionResult Create()
        {
            ViewData["ProgramId"] = new SelectList(_context.AcademicProgram, "ProgramId", "ProgramName");
            return View();
        }

        // POST: Lecturers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public async Task<IActionResult> Create(InputModel Input)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    PhoneNumber = Input.PhoneNumber,
                    CreatedAt = DateTime.Now,
                };

                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("New Committee account created.");

                    await _userManager.AddToRoleAsync(user, "Committee");

                    var committee = new Committee
                    {
                        CommitteeName = user.Name,
                        CommitteeId = Input.CommitteeId,
                        ApplicationUserId = user.Id,
                        ProgramId = Input.ProgramId,
                    };

                    _context.committee.Add(committee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewData["ProgramId"] = new SelectList(_context.AcademicProgram, "ProgramId", "ProgramName", Input.ProgramId);
            // If we got this far, something failed, redisplay form
            return View(Input);
        }

        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var committee = await _context.committee.Include(t => t.ApplicationUser).Include(t => t.AcademicProgram).ToListAsync();

            if (committee == null)
            {
                return NotFound();
            }

            var model = committee.Select(t => new ComitteeViewModel
            {
                CommitteeId = t.CommitteeId,
                CommitteeName = t.ApplicationUser.Name,
                email = t.ApplicationUser.Email,
                PhoneNumber = t.ApplicationUser.PhoneNumber,
                //ProgramName = t.AcademicProgram.ProgramName,
                ProgramId = t.ProgramId

            }).ToList();
            ViewData["ProgramNames"] = committee.ToDictionary(c => c.CommitteeId, c => c.AcademicProgram.ProgramName);
            return View(model);
        }

        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var committee = await _context.committee.Include(t => t.ApplicationUser).Include(t => t.AcademicProgram).FirstOrDefaultAsync(t => t.CommitteeId == id);

            if (committee == null)
            {
                return NotFound();
            }

            var model = new ComitteeViewModel
            {
                CommitteeId = committee.CommitteeId,
                CommitteeName = committee.ApplicationUser.Name,
                email = committee.ApplicationUser.Email,
                PhoneNumber = committee.ApplicationUser.PhoneNumber,
                //ProgramName = committee.AcademicProgram.ProgramName,
                ProgramId = committee.ProgramId
            };

            ViewData["Programs"] = new SelectList(await _context.AcademicProgram.ToListAsync(), "ProgramId", "ProgramName", committee.ProgramId);
            return View(model);
        }

        // POST: Committee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(ComitteeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var committee = await _context.committee.Include(t => t.ApplicationUser).Include(t => t.AcademicProgram).FirstOrDefaultAsync(t => t.CommitteeId == model.CommitteeId);

                if (committee == null)
                {
                    return NotFound();
                }

                committee.ApplicationUser.Email = model.email;
                committee.ApplicationUser.PhoneNumber = model.PhoneNumber;
                committee.ApplicationUser.Name = model.CommitteeName;
                committee.ProgramId = model.ProgramId;

                _context.Update(committee);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["Programs"] = new SelectList(await _context.AcademicProgram.ToListAsync(), "ProgramId", "ProgramName", model.ProgramId);
            return View(model);
        }


    }
}
