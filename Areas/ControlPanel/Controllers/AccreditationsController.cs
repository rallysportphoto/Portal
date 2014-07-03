/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portal.Models;

namespace Portal.Areas.ControlPanel.Controllers
{
    [Authorize(Roles = "Accreditations")]
    public class AccreditationsController : Controller
    {
        private SportDataContext db = new SportDataContext();

        // GET: /ControlPanel/Accreditations/
        public async Task<ActionResult> Index()
        {
            return View(await db.Accreditations
                .Include(a=>a.Media)
                .Include(a=>a.Tag)
                .Include(a=>a.User)                
                    .ToListAsync());
        }

        // GET: /ControlPanel/Accreditations/Details/5
        public ActionResult Validate(int? id)
        {
            return RedirectToAction("Edit", new { id = id });
        }

        // GET: /ControlPanel/Accreditations/Create
        public ActionResult Create()
        {
            return View(new Accreditation()
            {
                Media = new List<MediaInfoEntry>()
                {
                    new MediaInfoEntry()
                },
                User = new User()
            });
        }

        // POST: /ControlPanel/Accreditations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Accreditation accreditation)
        {
            if (ModelState.IsValid)
            {
                db.Accreditations.Add(accreditation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(accreditation);
        }

        // GET: /ControlPanel/Accreditations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accreditation accreditation = await db.Accreditations
                .Include(a=>a.User)
                .Include(a=>a.Media)
                .FirstOrDefaultAsync(a=> a.Id == id);
            if (accreditation == null)
            {
                return HttpNotFound();
            }
            return View(accreditation);
        }

        // POST: /ControlPanel/Accreditations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Accreditation accreditation)
        {
            if (ModelState.IsValid)
            {
                accreditation.Valid = true;
                db.Entry(accreditation).State = EntityState.Modified;
                db.Entry(accreditation.User).State = EntityState.Modified;
                foreach(var media in accreditation.Media)
                    db.Entry(media).State = EntityState.Modified;

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(accreditation);
        }

        // GET: /ControlPanel/Accreditations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Accreditation accreditation = await db.Accreditations.FindAsync(id);
            if (accreditation == null)
            {
                return HttpNotFound();
            }
            return View(accreditation);
        }

        // POST: /ControlPanel/Accreditations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Accreditation accreditation = await db.Accreditations.FindAsync(id);
            db.Accreditations.Remove(accreditation);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> GeneratePrintForms(int id)
        {
            var accreditation = await db.Accreditations
                .Include(a=>a.User)
                .Include(a=>a.Media)                
                .Include(a=>a.Tag)                
                .FirstAsync(o => o.Id == id);
            var eventTag = ((EventTag)accreditation.Tag);
            await db.Entry(eventTag).Reference(t=>t.Event).LoadAsync();

            if (eventTag.Event.TimingSystemId.HasValue)
            {
                var connector = new TrackMePro.PortalConnector();


                var serverAccreditation = new TrackMePro.Accreditation();
                serverAccreditation.Address = accreditation.User.Address;
                serverAccreditation.BirthDate = accreditation.User.BirthDate;
                serverAccreditation.Car = accreditation.Car;
                serverAccreditation.Email = accreditation.User.Email;
                serverAccreditation.FirstName = accreditation.User.FirstName;
                serverAccreditation.LastName = accreditation.User.LastName;
                serverAccreditation.Location = accreditation.User.Location;
                serverAccreditation.Passport = accreditation.User.Passport;
                serverAccreditation.Phone = accreditation.User.Phone;
                serverAccreditation.Id = accreditation.TimingSystemId.GetValueOrDefault();
                serverAccreditation.Media = accreditation.Media.Select(m => new TrackMePro.MediaInfoEntry()
                {
                    Edition = m.Edition,
                    Email = m.Email,
                    Fax = m.Fax,
                    Frequency = m.Frequency,
                    Media = m.Media,
                    Organisation = m.Organisation,
                    Phone = m.Phone,
                    Position = m.Position,
                    Region = m.Region,
                    Type = ((TrackMePro.MediaType)(int)m.Type),
                    Web = m.Web 
                }).ToArray();


                accreditation.TimingSystemId = connector.RegisterAccreditation(((EventTag)accreditation.Tag).Event.TimingSystemId.Value, accreditation.TimingSystemId, serverAccreditation);                
                await db.SaveChangesAsync();

                return RedirectToAction("DownloadPrintForm", new { id = id });
            }
            return View("TimingNotConfigured");
        }

        public async Task<ActionResult> DownloadPrintForm(int id)
        {
             var accreditation = await db.Accreditations
                .Include(a=>a.Tag)                
                .FirstAsync(o => o.Id == id);
             var eventTag = ((EventTag)accreditation.Tag);
             await db.Entry(eventTag).Reference(t => t.Event).LoadAsync();

             if (accreditation.TimingSystemId.HasValue && eventTag.Event.TimingSystemId.HasValue)
            {
                var connector = new TrackMePro.PortalConnector();

                return File(connector.GetAccreditationPrintForm(eventTag.Event.TimingSystemId.Value, accreditation.TimingSystemId.Value), "application/pdf");
            }
            return View("TimingNotConfigured");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
