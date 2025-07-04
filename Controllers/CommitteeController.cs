﻿using System;
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

        public async Task<IActionResult> PendingApplications()
        {
            var students = await _context.student
                .Where(s => s.applicationStatus == "Pending")
                .Include(s => s.lecturer) // Include the lecturer if needed
                .ToListAsync();

            var model = students.Select(s => new StudentApplicationViewModel
            {
                StudentId = s.StudentId,
                StudentName = s.StudentName,
                ApplicationStatus = s.applicationStatus,
                LecturerName = s.lecturer.LecturerName,
                LecturerId = s.lecturer.LecturerId
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveApplication(string studentId)
        {
            var student = await _context.student.FindAsync(studentId);
            if (student != null)
            {
                student.applicationStatus = "Approved";
                _context.Update(student);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(PendingApplications));
        }

        [Route("ProposalsWithoutEvaluators")]
        public async Task<IActionResult> ProposalsWithoutEvaluators()
        {
            var proposals = await _context.proposal
                .Where(p => p.LecturerId1 == null || p.LecturerId2 == null)
                .Include(p => p.student)
                .ToListAsync();

            var model = proposals.Select(p => new ProposalViewModel
            {
                ProposalId = p.ProposalId,
                Title = p.title,
                Type = p.type,
                LecturerId1 = p.LecturerId1,
                LecturerId2 = p.LecturerId2,
                StudentName = p.student.StudentName
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> AssignEvaluators(int id)
        {
            var proposal = await _context.proposal.Include(p => p.student).FirstOrDefaultAsync(p => p.ProposalId == id);

            if (proposal == null)
            {
                return NotFound();
            }

            var availableEvaluators = await _context.lecturer.Include(l => l.ApplicationUser)
                .Where(l =>l.domain == proposal.type && l.LecturerId != proposal.LecturerId1 && l.LecturerId != proposal.LecturerId2 && l.LecturerId != proposal.student.LecturerId)
                .ToListAsync();

            ViewBag.Evaluators = new SelectList(availableEvaluators, "LecturerId", "ApplicationUser.Name");

            var model = new ProposalViewModel
            {
                ProposalId = proposal.ProposalId,
                Title = proposal.title,
                Type = proposal.type,
                LecturerId1 = proposal.LecturerId1,
                LecturerId2 = proposal.LecturerId2,
                StudentName = proposal.student.StudentName
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignEvaluators(ProposalViewModel model)
        {
            var proposal = await _context.proposal.FindAsync(model.ProposalId);

            if (proposal == null)
            {
                return NotFound();
            }

            proposal.LecturerId1 = model.LecturerId1;
            proposal.LecturerId2 = model.LecturerId2;

            _context.proposal.Update(proposal);
            await _context.SaveChangesAsync();

            return RedirectToAction("ProposalList", "Proposal");
        }

        [HttpPost]
        public async Task<IActionResult> AutoAssignEvaluators(int id)
        {
            var proposal = await _context.proposal.Include(p => p.student).FirstOrDefaultAsync(p => p.ProposalId == id);
            if (proposal == null)
            {
                return NotFound();
            }

            var availableEvaluators = await _context.lecturer
                .Where(l => l.LecturerId != proposal.student.LecturerId)
                .ToListAsync();

            var suitableEvaluators = availableEvaluators
                .Where(l => l.domain.Contains(proposal.type))
                .ToList();

            if (suitableEvaluators.Count < 2)
            {
                ModelState.AddModelError("", "Not enough evaluators available.");
                return RedirectToAction("AssignEvaluators", new { id = id });
            }

            proposal.LecturerId1 = suitableEvaluators[0].LecturerId;
            proposal.LecturerId2 = suitableEvaluators[1].LecturerId;

            _context.proposal.Update(proposal);
            await _context.SaveChangesAsync();

            return RedirectToAction("AssignEvaluators");
        }
    }
}
