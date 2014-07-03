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
using Portal.Services;

namespace Portal.Areas.ControlPanel.Controllers
{
    public class PartnersController : Controller
    {
        private SportDataContext db = new SportDataContext();

        // GET: /ControlPanel/Partners/
        public async Task<ActionResult> Index()
        {
            return View(await db.Partners.ToListAsync());
        }

        // GET: /ControlPanel/Partners/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View();
        }

        // POST: /ControlPanel/Partners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Desctiption,Global,Url,IsEnabled,Position")] Partner partner, string[] tags, HttpPostedFileBase logo)
        {
            if (ModelState.IsValid)
            {
                db.Partners.Add(partner);

                if (tags != null)
                {
                    partner.Tags = new List<Tag>();
                    var tagList = await db.Tags.ToListAsync();

                    foreach (var tag in tags)
                    {
                        partner.Tags.Add(tagList.First(t => t.Id == tag));
                    }
                }
                if (logo != null)
                    partner.Logo = DocumentService.CreateAutomaticFile(logo, DateTime.Now, title: partner.Name, hidden: true);

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(partner);
        }

        // GET: /ControlPanel/Partners/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = await db.Partners.Include(p=>p.Tags).FirstOrDefaultAsync(p=>p.Id==id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(partner);
        }

        // POST: /ControlPanel/Partners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Name,Desctiption,Global,Url,IsEnabled,Position")] Partner partner, string[] tags, HttpPostedFileBase logo)
        {
            db.Entry(partner).State = EntityState.Modified;
            await db.Entry(partner).Collection(n => n.Tags).LoadAsync();
            if (ModelState.IsValid)
            {
                var tagList = await db.Tags.ToListAsync();
                partner.Tags.Clear();
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        partner.Tags.Add(tagList.First(t => t.Id == tag));
                    }
                }
                if (logo != null)
                {
                    await db.Entry(partner).Reference(p => p.Logo).LoadAsync();
                    if(logo!=null)
                        partner.Logo = DocumentService.CreateAutomaticFile(logo, DateTime.Now, title: partner.Name, hidden: true);                    
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(partner);
        }

        // GET: /ControlPanel/Partners/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = await db.Partners.FirstOrDefaultAsync(p=>p.Id==id);
            if (partner == null)
            {
                return HttpNotFound();
            }
            return View(partner);
        }

        // POST: /ControlPanel/Partners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Partner partner = await db.Partners.FirstOrDefaultAsync(p => p.Id == id);
            db.Partners.Remove(partner);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
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
