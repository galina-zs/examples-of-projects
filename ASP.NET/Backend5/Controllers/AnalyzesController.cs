using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Backend5.Data;
using Backend5.Models;
using Backend5.Models.ViewModels;

namespace Backend5.Controllers
{
    public class AnalyzesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalyzesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Analyzes
        public async Task<IActionResult> Index(int? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Patient = patient;
            var analyzes = await this._context.Analyzes
                .Include(w => w.Patient)
                .Include(w => w.Lab)
                .Where(x => x.PatientId == patientId)
                .ToListAsync();

            return this.View(analyzes);
        }

        // GET: Analyzes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyzes
                .Include(a => a.Lab)
                .Include(a => a.Patient)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (analysis == null)
            {
                return NotFound();
            }

            return View(analysis);
        }

        // GET: Analyzes/Create
        public async Task<IActionResult> Create(Int32? patientId)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Patient = patient;
            this.ViewData["LabId"] = new SelectList(this._context.Labs, "Id", "Name");
            return this.View(new AnalysisCreateModel());
        }

        // POST: Analyzes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? patientId, AnalysisCreateModel model)
        {
            if (patientId == null)
            {
                return this.NotFound();
            }

            var patient = await this._context.Patients
                .SingleOrDefaultAsync(x => x.Id == patientId);

            if (patient == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var analysis = new Analysis
                {
                    PatientId = patient.Id,
                    LabId = model.LabId,
                    Type = model.Type,
                    Status = model.Status,
                    DateTime = model.DateTime
                };

                this._context.Add(analysis);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = patient.Id });
            }

            this.ViewBag.Patient = patient;
            this.ViewData["LabId"] = new SelectList(this._context.Labs, "Id", "Name", model.LabId);
            return this.View(model);
        }

        // GET: Analyzes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyzes.SingleOrDefaultAsync(m => m.Id == id);
            if (analysis == null)
            {
                return NotFound();
            }

            var model = new AnalysisEditModel
            {
                Type = analysis.Type,
                Status = analysis.Status,
                LabId = analysis.LabId,
                DateTime = analysis.DateTime
            };

            return this.View(model);
        }

        // POST: Analyzes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, AnalysisEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var analysis = await this._context.Analyzes.SingleOrDefaultAsync(m => m.Id == id);
            if (analysis == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                analysis.Type = model.Type;
                analysis.LabId = model.LabId;
                analysis.Status = model.Status;
                analysis.DateTime = model.DateTime;

                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { patientId = analysis.PatientId });
            }

            return this.View(model);
        }

        // GET: Analyzes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analysis = await _context.Analyzes
                .Include(a => a.Lab)
                .Include(a => a.Patient)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (analysis == null)
            {
                return NotFound();
            }
            this.ViewData["LabId"] = new SelectList(this._context.Labs, "Id", "Name");
            return View(analysis);
        }

        // POST: Analyzes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var analysis = await _context.Analyzes.SingleOrDefaultAsync(m => m.Id == id);
            _context.Analyzes.Remove(analysis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { patientId = analysis.PatientId });
        }
    }
}
