using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ADTest.Data;
using ADTest.Models;

namespace ADTest.Controllers
{
    public class AcademicProgramsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AcademicProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AcademicPrograms
        public async Task<IActionResult> Index()
        {
            return View(await _context.AcademicProgram.ToListAsync());
        }

        // GET: AcademicPrograms/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicProgram = await _context.AcademicProgram
                .FirstOrDefaultAsync(m => m.ProgramId == id);
            if (academicProgram == null)
            {
                return NotFound();
            }

            return View(academicProgram);
        }

        // GET: AcademicPrograms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AcademicPrograms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProgramId,ProgramName")] AcademicProgram academicProgram)
        {
            if (ModelState.IsValid)
            {
                _context.Add(academicProgram);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(academicProgram);
        }

        // GET: AcademicPrograms/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicProgram = await _context.AcademicProgram.FindAsync(id);
            if (academicProgram == null)
            {
                return NotFound();
            }
            return View(academicProgram);
        }

        // POST: AcademicPrograms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProgramId,ProgramName")] AcademicProgram academicProgram)
        {
            if (id != academicProgram.ProgramId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicProgram);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicProgramExists(academicProgram.ProgramId))
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
            return View(academicProgram);
        }

        // GET: AcademicPrograms/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academicProgram = await _context.AcademicProgram
                .FirstOrDefaultAsync(m => m.ProgramId == id);
            if (academicProgram == null)
            {
                return NotFound();
            }

            return View(academicProgram);
        }

        // POST: AcademicPrograms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var academicProgram = await _context.AcademicProgram.FindAsync(id);
            if (academicProgram != null)
            {
                _context.AcademicProgram.Remove(academicProgram);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicProgramExists(string id)
        {
            return _context.AcademicProgram.Any(e => e.ProgramId == id);
        }
    }
}
