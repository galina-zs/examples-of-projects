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
    public class PlacementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacementsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Placements
        public async Task<IActionResult> Index(int? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;
            var placements = await this._context.Placements
                .Include(w => w.Ward)
                .Include(w => w.Patient)
                .Include(w => w.Hospital)
                .Where(x => x.WardId == wardId)
                .ToListAsync();

            return this.View(placements);
        }

        // GET: Placements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Hospital)
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }

            return View(placement);
        }

        // GET: Placements/Create
        public async Task<IActionResult> Create(Int32? wardId)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Ward = ward;
            this.ViewData["PatientId"] = new SelectList(this._context.Patients, "Id", "Name");
            return this.View(new PlacementCreateModel());
        }

        // POST: Placements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? wardId, PlacementCreateModel model)
        {
            if (wardId == null)
            {
                return this.NotFound();
            }

            var ward = await this._context.Wards
                .SingleOrDefaultAsync(x => x.Id == wardId);

            if (ward == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var placement = new Placement
                {
                    WardId = ward.Id,
                    HospitalId = ward.HospitalId,
                    Hospital = ward.Hospital,
                    PatientId = model.PatientId,
                    Bed = model.Bed
                };

                this._context.Add(placement);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = ward.Id });
            }

            this.ViewBag.Ward = ward;
            this.ViewData["PatientId"] = new SelectList(this._context.Patients, "Id", "Name", model.PatientId);
            return this.View(model);
        }

        // GET: Placements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }
            var model = new PlacementEditModel
            {
                PatientId = placement.PatientId,
                Bed = placement.Bed
            };

            return this.View(model);
        }

        // POST: Placements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, PlacementEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var placement = await this._context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                placement.Bed = model.Bed;
                placement.PatientId = model.PatientId;

                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = placement.WardId });
            }

            return this.View(model);
        }

        // GET: Placements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placement = await _context.Placements
                .Include(p => p.Hospital)
                .Include(p => p.Patient)
                .Include(p => p.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (placement == null)
            {
                return NotFound();
            }
            this.ViewData["PatientId"] = new SelectList(this._context.Patients, "Id", "Name");
            return View(placement);
        }

        // POST: Placements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var placement = await _context.Placements.SingleOrDefaultAsync(m => m.Id == id);
            _context.Placements.Remove(placement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { wardId = placement.WardId });
        }
    }
}
