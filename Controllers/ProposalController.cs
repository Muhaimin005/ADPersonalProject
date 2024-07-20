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
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace ADTest.Controllers
{
    public class ProposalController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ProposalController> _logger;
        private readonly ApplicationDbContext _context;

        public ProposalController
        (
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ProposalController> logger,
            ApplicationDbContext context
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        // STUDENT SIDE - START //
        public async Task<IActionResult> SubmitProposal()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.IC;
            var student = await _context.student.FirstOrDefaultAsync(s => s.StudentId == userId);

            ProposalInputModel proposal = new ProposalInputModel()
            {
                StudentId = userId,
                semester = student.semester,
                session = student.academicSession
            };

            return View(proposal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitProposal(ProposalInputModel Input)
        {
            if (ModelState.IsValid)
            {
                if (Input.proposalForm != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await Input.proposalForm.CopyToAsync(ms);
                        var fileData = ms.ToArray();
                        // Save file data to the model
                        var proposal = new Proposal
                        {
                            StudentId = Input.StudentId,
                            title = Input.title,
                            type = Input.type,
                            semester = Input.semester,
                            session = Input.session,
                            status = "Pending",
                            proposalForm = fileData,
                        };
                        _context.Add(proposal);
                        await _context.SaveChangesAsync();
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(Input);
        }

        public async Task<IActionResult> ViewProposal()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.IC;

            var proposal = await _context.proposal
                .Where(p => p.StudentId == userId)
                .ToListAsync();

            return View(proposal);
        }

		public IActionResult ViewProposalForm(int id)
		{
            var proposal = _context.proposal.FirstOrDefault(p => p.ProposalId == id);

            var base64Pdf = Convert.ToBase64String(proposal.proposalForm);
            ViewBag.PdfData = base64Pdf;

            return View(proposal);
		}
		// STUDENT SIDE - END //

		// COMMITTEE SIDE - START //
		public async Task<IActionResult> ProposalList()
        {
            var proposalList = await _context.proposal
                                     .Include(p => p.student)
                                     .ToListAsync();

            return View(proposalList);
        }
        // COMMITTEE SIDE - END //

        // SUPERVISOR SIDE - START //
        // SUPERVISOR SIDE - END //

        // EVALUATOR SIDE - START //
        // EVALUATOR SIDE - END //
    }
}
