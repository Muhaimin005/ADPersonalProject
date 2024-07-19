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

        // GENERAL FUNCTIONS - START //
        public async Task<ApplicationUser> GetUserByLecturerIdAsync(string lecturerId)
        {
            return await _userManager.Users.SingleOrDefaultAsync(u => u.IC == lecturerId);
        }
        // GENERAL FUNCTIONS - END //

        // COMMITTEE SIDE - START //
        public async Task<IActionResult>AssignDomain()
        {
            List<Lecturer> lecturerList = _context.lecturer.ToList();
            return View(lecturerList);
        }

		public IActionResult UpdateDomain(string status, string lecturerId)
		{
			var lecturer = _context.lecturer.Find(lecturerId);
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
        // COMMITTEE SIDE - END //

        // ADMIN SIDE - START //
        public IActionResult CreateCommittee()
        {
            ViewData["ProgramId"] = new SelectList(_context.AcademicProgram, "ProgramId", "ProgramName");
            return View();
        }

        // POST: Lecturers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCommittee(CommitteeInputModel Input)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    IC = "dummy",
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var commitee = await _context.committee.FindAsync(id);
            if (commitee == null)
            {
                return NotFound();
            }

            _context.committee.Remove(commitee);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageCommittee");
        }

        public async Task<IActionResult> ManageCommittee()
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
                ProgramId = t.ProgramId
            }).ToList();

            var programNames = committee.ToDictionary(c => c.CommitteeId, c => c.AcademicProgram != null ? c.AcademicProgram.ProgramName : "No Program Assigned");

            ViewData["ProgramNames"] = programNames;
            ViewData["ProgramId"] = new SelectList(_context.AcademicProgram, "ProgramId", "ProgramName");

            return View(model);
        }


        public async Task<IActionResult> EditCommittee(string id)
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
        public async Task<IActionResult> EditCommittee(ComitteeViewModel model)
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

                return RedirectToAction("ManageCommittee");
            }
            ViewData["Programs"] = new SelectList(await _context.AcademicProgram.ToListAsync(), "ProgramId", "ProgramName", model.ProgramId);
            return View(model);
        }

        public IActionResult AssignCommittee() // <--- Finish this pass ID and take only specific bill
        {
            List<Lecturer> lecturer = _context.lecturer.ToList();

            return View(lecturer);
        }

        public async Task<IActionResult> AssignYes(string lecturerId)
        {
            var lecturer = await _context.lecturer.Include(l => l.ApplicationUser).FirstOrDefaultAsync(l => l.LecturerId == lecturerId);
            if (lecturer != null)
            {
                lecturer.isCommittee = "Yes";
                _context.SaveChanges();

                var user = await GetUserByLecturerIdAsync(lecturerId);

                await _userManager.RemoveFromRoleAsync(user, "Lecturer");
                await _userManager.AddToRoleAsync(user, "Committee");

                // Create committee record
                var committee = new Committee
                {
                    CommitteeName = lecturer.ApplicationUser.Name,
                    CommitteeId = lecturerId, // Assuming lecturerId can be used as CommitteeId
                    ApplicationUserId = lecturer.ApplicationUserId,
                    ProgramId = null // Initially, no program assigned
                };

                _context.lecturer.Update(lecturer);
                _context.committee.Add(committee);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AssignCommittee");
        }

        public async Task<IActionResult> AssignNo(string lecturerId)
        {
            var lecturer = await _context.lecturer.FindAsync(lecturerId);
            if (lecturer != null)
            {
                // Set lecturer's isCommittee to "No"
                lecturer.isCommittee = "No";
                _context.Update(lecturer);

                // Find the committee record associated with this lecturer
                var committee = await _context.committee.SingleOrDefaultAsync(c => c.ApplicationUserId == lecturer.ApplicationUserId);
                if (committee != null)
                {
                    // Delete the committee record
                    _context.committee.Remove(committee);
                }

                // Update user roles
                var user = await GetUserByLecturerIdAsync(lecturerId);
                if (user != null)
                {
                    await _userManager.RemoveFromRoleAsync(user, "Committee");
                    await _userManager.AddToRoleAsync(user, "Lecturer");
                }

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("AssignCommittee");
        }


        [HttpPost]
        public async Task<IActionResult> AssignProgram(string committeeId, string ProgramId)
        {
            var committee = await _context.committee.Include(t => t.ApplicationUser).Include(t => t.AcademicProgram).FirstOrDefaultAsync(t => t.CommitteeId == committeeId);

            if (committee == null)
            {
                return NotFound();
            }

            committee.ProgramId = ProgramId;
            _context.Update(committee);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageCommittee");
        }
        // ADMIN SIDE - END //

    }
}
