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
    public class WardStaffsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WardStaffsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WardStaffs
        public async Task<IActionResult> Index(Int32? wardId)
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
            var wardStaffs = await this._context.WardStaffs
                .Include(w => w.Ward)
                .Include(w => w.Hospital)
                .Where(x => x.WardId == wardId)
                .ToListAsync();

            return this.View(wardStaffs);
        }

        // GET: WardStaffs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaffs
                .Include(w => w.Hospital)
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaff == null)
            {
                return NotFound();
            }

            return View(wardStaff);
        }

        // GET: WardStaffs/Create
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
            return this.View(new WardStaffCreateModel());
        }

        // POST: WardStaffs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? wardId, WardStaffCreateModel model)
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
                var wardStaff = new WardStaff
                {
                    WardId = ward.Id,
                    HospitalId = ward.HospitalId,
                    Hospital = ward.Hospital,
                    Name = model.Name,
                    Position = model.Position
                };

                this._context.Add(wardStaff);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = ward.Id });
            }

            this.ViewBag.Ward = ward;
            return this.View(model);
        }

        // GET: WardStaffs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaffs.SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaff == null)
            {
                return NotFound();
            }
            var model = new WardStaffEditModel
            {
                Name = wardStaff.Name,
                Position = wardStaff.Position
            };

            return this.View(model);
        }

        // POST: WardStaffs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Int32? id, WardStaffEditModel model)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var wardStaff = await this._context.WardStaffs.SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaff == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                wardStaff.Name = model.Name;
                wardStaff.Position = model.Position;
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { wardId = wardStaff.WardId });
            }

            return this.View(model);
        }

        // GET: WardStaffs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var wardStaff = await _context.WardStaffs
                .Include(w => w.Hospital)
                .Include(w => w.Ward)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (wardStaff == null)
            {
                return NotFound();
            }

            return View(wardStaff);
        }

        // POST: WardStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wardStaff = await _context.WardStaffs.SingleOrDefaultAsync(m => m.Id == id);
            _context.WardStaffs.Remove(wardStaff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { wardId = wardStaff.WardId });
        }
    }
}
