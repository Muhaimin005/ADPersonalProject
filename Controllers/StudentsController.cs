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

namespace ADTest.Controllers
{
    public class StudentsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LecturersController> _logger;
        private readonly ApplicationDbContext _context;

        public StudentsController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<LecturersController> logger, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> SelectSupervisor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var student = await _context.student
                .FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);

            if (student == null)
            {
                return NotFound();
            }

            if (student.LecturerId == null)
            {
                var lecturers = await _context.lecturer.Include(t=> t.ApplicationUser).ToListAsync();

                var model = lecturers.Select(t => new LecturerViewModel
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
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Apply(string lecturerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var student = await _context.student
                .FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);

            if (student == null)
            {
                return NotFound();
            }

            student.LecturerId = lecturerId;
            student.applicationStatus = "Pending";

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Students", new { id = user.Id });
        }

        [HttpPost]
        public async Task<IActionResult> CancelApplication(string studentId)
        {
            var student = await _context.student
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                return NotFound();
            }

            student.LecturerId = null;
            student.applicationStatus = "Cancelled";

            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = student.ApplicationUserId });
        }



        // GET: Students/Details/5
        [Route("Details")]
        public async Task<IActionResult> Details(string id)
        {
            var student = await _context.student
                .Include(s => s.ApplicationUser)
                .Include(s => s.AcademicProgram)
                .Include(s => s.lecturer)
                .FirstOrDefaultAsync(s => s.ApplicationUserId  == id);

            if (student == null)
            {
                return NotFound();
            }

            ViewData["ProgramName"] = student.AcademicProgram?.ProgramName ?? "No Program Assigned";

            var model = new StudentViewModel
            {
                StudentId = student.ApplicationUser.IC,
                StudentName = student.ApplicationUser.Name,
                StudentEmail = student.ApplicationUser.Email,
                StudentPhone = student.ApplicationUser.PhoneNumber,
                ProgramName = student.AcademicProgram?.ProgramName ?? "No Program Assigned",
                LecturerName = student.LecturerId != null ? student.lecturer.LecturerName : "No supervisor registered",
                ApplicationStatus = student.applicationStatus,
                semester = student.semester,
                academicSession = student.academicSession
            };

            return View(model);
        }

        // GET: Students/Create
        [Route("Create")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(); // Handle case where user is not found
            }

            var viewModel = new StudentInputModel
            {
                StudentName = user.Name,
                StudentPhone = user.PhoneNumber,
                StudentEmail = user.Email
            };

            ViewData["ProgramId"] = new SelectList(_context.AcademicProgram, "ProgramId", "ProgramName");
            //ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id");
            //ViewData["LecturerId"] = new SelectList(_context.lecturer, "LecturerId", "LecturerName");
            return View(viewModel);
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentInputModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound(); // Handle case where user is not found
            };

            if (ModelState.IsValid)
            {
                var student = new Student
                {
                    StudentId = user.IC,
                    StudentName = input.StudentName,
                    ApplicationUserId = user.Id,
                    LecturerId = null,
                    ProgramId = input.ProgramId,
                    applicationStatus = "No Supervisor Application recorded",
                    semester = input.semester,
                    academicSession = input.academicSession

                };

                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }

            ViewData["ProgramId"] = new SelectList(_context.AcademicProgram, "ProgramId", "ProgramName", input.ProgramId);
            //ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", input.ApplicationUserId);
            //ViewData["LecturerId"] = new SelectList(_context.lecturer, "LecturerId", "LecturerId", student.LecturerId);
            return View(input);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["ProgramId"] = new SelectList(_context.AcademicProgram, "ProgramId", "ProgramId", student.ProgramId);
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", student.ApplicationUserId);
            ViewData["LecturerId"] = new SelectList(_context.lecturer, "LecturerId", "LecturerId", student.LecturerId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StudentId,StudentName,ProgramId,LecturerId,ApplicationUserId,applicationStatus")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            ViewData["ProgramId"] = new SelectList(_context.AcademicProgram, "ProgramId", "ProgramId", student.ProgramId);
            ViewData["ApplicationUserId"] = new SelectList(_context.Users, "Id", "Id", student.ApplicationUserId);
            ViewData["LecturerId"] = new SelectList(_context.lecturer, "LecturerId", "LecturerId", student.LecturerId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.student
                .Include(s => s.AcademicProgram)
                .Include(s => s.ApplicationUser)
                .Include(s => s.lecturer)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var student = await _context.student.FindAsync(id);
            if (student != null)
            {
                _context.student.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(string id)
        {
            return _context.student.Any(e => e.StudentId == id);
        }

        public async Task<IActionResult> ViewSupervisor()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var student = await _context.student
                .FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);

            if (student == null)
            {
                return NotFound();
            }

            if (student.LecturerId != null)
            {
                var lecturers = await _context.lecturer
                    .Include(t => t.ApplicationUser)
                    .Where(t => t.LecturerId == student.LecturerId)
                    .ToListAsync();

                var model = lecturers.Select(t => new LecturerViewModel
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
            else
            {
                return View();
            }

        }
    }
}
