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
using System.Data;

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
            [Display(Name = "Fieldofstudy")]
            public string FieldofStudy { get; set; }
        }

        // GET: Lecturers
        [Route("Index")]
        public async Task<IActionResult> Index()
        {
            var lecturer = await _context.lecturer.Include(t => t.ApplicationUser).ToListAsync(); 

            if (lecturer == null)
            {
                return NotFound();
            }

            var model = lecturer.Select(t=> new LecturerViewModel 
            {
                lecturerId = t.LecturerId,
                LecturerName = t.ApplicationUser.Name,
                email = t.ApplicationUser.Email,
                PhoneNumber = t.ApplicationUser.PhoneNumber,
                LecturerAddress = t.LecturerAddress,
                FieldofStudy = t.FieldofStudy,
                Domain = t.domain,
                IsCommittee = t.isCommittee,
            }).ToList();

            return View(model);
        }

        // GET: Lecturers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var lecturer = await _context.lecturer.Include(t => t.ApplicationUser).FirstOrDefaultAsync(t => t.LecturerId == id);

            if (lecturer == null)
            {
                return NotFound();
            }

            var model = new LecturerViewModel
            {
                lecturerId = lecturer.LecturerId,
                LecturerName = lecturer.ApplicationUser.Name,
                email = lecturer.ApplicationUser.Email,
                PhoneNumber = lecturer.ApplicationUser.PhoneNumber,
                LecturerAddress = lecturer.LecturerAddress,
                FieldofStudy = lecturer.FieldofStudy,
                Domain = lecturer.domain

            };

            return View(model);
        }

        // GET: Lecturers/Create
        [Route("Create")]
        public IActionResult Create()
        {
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
                    IC = Input.LecturerID,
                    UserName = Input.Email,
                    Email = Input.Email,
                    Name = Input.Name,
                    PhoneNumber = Input.PhoneNumber,
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
                        domain = "Pending",
                        isCommittee = "No",
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

            // If we got this far, something failed, redisplay form
            return View(Input);
        }

        // GET: Lecturers/Edit/5
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var lecturer = await _context.lecturer.Include(t=>t.ApplicationUser).FirstOrDefaultAsync(t=>t.LecturerId == id);

            if (lecturer == null)
            {
                return NotFound();
            }

            var model = new LecturerViewModel
            {
                lecturerId = lecturer.LecturerId,
                LecturerName = lecturer.ApplicationUser.Name,
                email = lecturer.ApplicationUser.Email,
                PhoneNumber = lecturer.ApplicationUser.PhoneNumber,
                LecturerAddress = lecturer.LecturerAddress,
                FieldofStudy = lecturer.FieldofStudy,
                Domain=lecturer.domain

            };

            return View(model);
        }

        // POST: Lecturers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public async Task<IActionResult> Edit(LecturerViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lecturer = await _context.lecturer.Include(t => t.ApplicationUser).FirstOrDefaultAsync(t => t.LecturerId == model.lecturerId);

                if (lecturer == null)
                {
                    return NotFound();
                }

                lecturer.ApplicationUser.Email = model.email;
                lecturer.ApplicationUser.PhoneNumber = model.PhoneNumber;
                lecturer.ApplicationUser.Name = model.LecturerName;
                lecturer.LecturerName = model.LecturerName;
                lecturer.LecturerAddress = model.LecturerAddress;
                lecturer.FieldofStudy = model.FieldofStudy;
                lecturer.domain = model.Domain;                
                
                _context.Update(lecturer);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Lecturers/Delete/5
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var lecturer = await _context.lecturer.Include(t => t.ApplicationUser).FirstOrDefaultAsync(t => t.LecturerId == id);

            if (lecturer == null)
            {
                return NotFound();
            }

            var model = new LecturerViewModel
            {
                lecturerId = lecturer.LecturerId,
                LecturerName = lecturer.ApplicationUser.Name,
                email = lecturer.ApplicationUser.Email,
                PhoneNumber = lecturer.ApplicationUser.PhoneNumber,
                LecturerAddress = lecturer.LecturerAddress,
                FieldofStudy = lecturer.FieldofStudy,
                Domain = lecturer.domain

            };

            return View(model);
        }

        // POST: Lecturers/Delete/5
        [Route("Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(LecturerViewModel model)
        {
            var lecturer = await _context.lecturer.Include(t => t.ApplicationUser).FirstOrDefaultAsync(t => t.LecturerId == model.lecturerId);
            if (lecturer != null)
            {

                if (lecturer == null)
                {
                    return NotFound();
                }
                else 
                {
                    var user = await _userManager.FindByIdAsync(lecturer.ApplicationUser.Id);

                    lecturer.ApplicationUser.Email = model.email;
                    lecturer.ApplicationUser.PhoneNumber = model.PhoneNumber;
                    lecturer.ApplicationUser.Name = model.LecturerName;
                    lecturer.LecturerName = model.LecturerName;
                    lecturer.LecturerAddress = model.LecturerAddress;
                    lecturer.FieldofStudy = model.FieldofStudy;
                    lecturer.domain = model.Domain;

                    _context.lecturer.Remove(lecturer);

                    var result = await _userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }           
            }

            //await _context.SaveChangesAsync();
            return View();
        }

        private bool LecturerExists(string id)
        {
            return _context.lecturer.Any(e => e.LecturerId == id);
        }

        public async Task<ApplicationUser> GetUserByLecturerIdAsync(string lecturerId)
        {
            return await _userManager.Users.SingleOrDefaultAsync(u => u.IC == lecturerId);
        }

        [Route("MyStudents")]
        public async Task<IActionResult> MyStudents()
        {
            var userId = _userManager.GetUserId(User); // Get the logged-in user's ID
            var lecturer = await _context.lecturer
                .Include(l => l.ApplicationUser)
                .FirstOrDefaultAsync(l => l.ApplicationUser.Id == userId);

            if (lecturer == null)
            {
                return NotFound("Lecturer not found for the logged-in user.");
            }

            var lecturerId = lecturer.LecturerId; // Get the LecturerId
            var students = await _context.student
                .Where(s => s.LecturerId == lecturerId)
                .Include(s => s.lecturer)
                .ThenInclude(l => l.ApplicationUser)
                .Include(s => s.proposal) // Include proposals
                .ToListAsync();

            var model = students.Select(s => new MyStudentViewModel
            {
                StudentId = s.StudentId,
                StudentName = s.StudentName,
                Semester = s.semester,
                academicSession = s.academicSession,
                ProposalId = s.proposal.ProposalId,
                ProposalStatus = s.proposal.status,
            }).ToList();

            return View(model);
        }

        [Route("ViewProposal")]
        public async Task<IActionResult> ViewProposal(int id)
        {
            var proposal = await _context.proposal
                .Include(p => p.student)
                .FirstOrDefaultAsync(p => p.ProposalId == id);

            if (proposal == null)
            {
                return NotFound();
            }

            return View(proposal);
        }

        public async Task<IActionResult> DownloadProposal(int id)
        {
            var proposal = await _context.proposal
                .FirstOrDefaultAsync(p => p.ProposalId == id);

            if (proposal == null || proposal.proposalForm == null)
            {
                return NotFound();
            }

            return File(proposal.proposalForm, "application/octet-stream", "ProposalForm.pdf");
        }



    }
}
