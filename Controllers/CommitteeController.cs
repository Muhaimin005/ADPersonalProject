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
	}
}
