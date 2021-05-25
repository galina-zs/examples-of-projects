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
    public class DoctorPatientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DoctorPatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DoctorPatients
        public async Task<IActionResult> Index(int? doctorId)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors
                .SingleOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            var items = await this._context.DoctorPatients
                .Include(h => h.Doctor)
                .Include(h => h.Patient)
                .Where(x => x.DoctorId == doctor.Id)
                .ToListAsync();
            this.ViewBag.Doctor = doctor;
            return this.View(items);
        }

        // GET: DoctorPatients/Create
        public async Task<IActionResult> Create(int? doctorId)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors
                .SingleOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            this.ViewBag.Doctor = doctor;
            this.ViewData["PatientId"] = new SelectList(this._context.Patients, "Id", "Name");
            return this.View(new DoctorPatientCreateModel());
        }

        // POST: DoctorPatients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Int32? doctorId, DoctorPatientCreateModel model)
        {
            if (doctorId == null)
            {
                return this.NotFound();
            }

            var doctor = await this._context.Doctors
                .SingleOrDefaultAsync(x => x.Id == doctorId);

            if (doctor == null)
            {
                return this.NotFound();
            }

            if (this.ModelState.IsValid)
            {
                var doctorPatient = new DoctorPatient
                {
                    DoctorId = doctor.Id,
                    PatientId = model.PatientId
                };

                this._context.Add(doctorPatient);
                await this._context.SaveChangesAsync();
                return this.RedirectToAction("Index", new { doctorId = doctor.Id });
            }

            this.ViewBag.Doctor = doctor;
            this.ViewData["PatientId"] = new SelectList(this._context.Patients, "Id", "Name", model.PatientId);
            return this.View(model);
        }

        // GET: DoctorPatients/Delete/5
        public async Task<IActionResult> Delete(Int32? doctorId, Int32? patientId)
        {
            if (doctorId == null || patientId == null)
            {
                return this.NotFound();
            }

            var doctorPatient = await this._context.DoctorPatients
                .Include(h => h.Doctor)
                .Include(h => h.Patient)
                .SingleOrDefaultAsync(m => m.DoctorId == doctorId && m.PatientId == patientId);
            if (doctorPatient == null)
            {
                return this.NotFound();
            }

            return this.View(doctorPatient);
        }

        // POST: DoctorPatients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Int32 doctorId, Int32 patientId)
        {
            var doctorPatient = await this._context.DoctorPatients.SingleOrDefaultAsync(m => m.DoctorId == doctorId && m.PatientId == patientId);
            this._context.DoctorPatients.Remove(doctorPatient);
            await this._context.SaveChangesAsync();
            return this.RedirectToAction("Index", new { doctorId = doctorId });
        }
    }
}
