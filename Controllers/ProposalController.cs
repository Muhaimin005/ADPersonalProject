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
            var userId = user.Id;

            ProposalInputModel proposal = new ProposalInputModel()
            {
                StudentId = userId,
            };

            return View(proposal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SUbmitProposal(ProposalInputModel Input)
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
        // STUDENT SIDE - END //

        // COMMITTEE SIDE - START //
        // COMMITTEE SIDE - END //

        // SUPERVISOR SIDE - START //
        // SUPERVISOR SIDE - END //

        // EVALUATOR SIDE - START //
        // EVALUATOR SIDE - END //
    }
}
