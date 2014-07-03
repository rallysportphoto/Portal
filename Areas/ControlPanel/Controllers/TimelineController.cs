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
using Portal.Areas.ControlPanel.Helpers;
using Portal.Services;

namespace Portal.Areas.ControlPanel.Controllers
{
    [Authorize(Roles = "Events")]
    public partial class TimelineController : Controller
    {
        private SportDataContext db = new SportDataContext();

        // GET: /ControlPanel/Timeline/
        public async Task<ActionResult> Index()
        {
            return View(await db.Events.ToListAsync());
        }      

        // GET: /ControlPanel/Timeline/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ControlPanel/Timeline/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Season,Title,StartDate,FinishDate,Place")] Event @event, int[] series)
        {
            if (ModelState.IsValid)
            {
                db.Events.Add(@event);
                @event.Series = new List<Series>();
                var seriesList = await db.Series.Where(s => s.Season == @event.Season).ToListAsync();
                series = series ?? new int[0];                
                foreach (var id in series)
                {
                    @event.Series.Add(seriesList.First(s => s.Id == id));
                }                
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: /ControlPanel/Timeline/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.Include(e=>e.Series).FirstOrDefaultAsync(n => n.Id == id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            if (@event is OfficialEvent)
            {
                await db.Entry(@event as OfficialEvent).Collection("SubEvents").LoadAsync();
                foreach (var s in (@event as OfficialEvent).SubEvents)
                {
                    await db.Entry(s).Reference(o => o.Map).LoadAsync();
                    await db.Entry(s).Reference(o => o.Event).LoadAsync();
                    await db.Entry(s).Reference(o => o.Series).LoadAsync();
                    await db.Entry(s).Reference(o => o.Tag).LoadAsync();
                }                
            }

            ViewBag.Series = await db.Series.Include(s => s.Discipline).Where(s=>s.Season == @event.Season).ToListAsync();
            return View(@event);
        }

        // POST: /ControlPanel/Timeline/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit([ModelBinder(typeof(ModelTypeBinder)), Bind(Exclude = "Series")]Event @event, int[] series, HttpPostedFileBase introFile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.Entry<Event>(@event).Collection(e => e.Series).Load();
                var seriesList = await db.Series.Where(s => s.Season == @event.Season).ToListAsync();
                series = series ?? new int[0];
                @event.Series.Clear();
                foreach (var id in series)
                {
                    @event.Series.Add(seriesList.First(s => s.Id == id));
                }

                if(introFile!=null) {
                    if (@event is FeaturableEvent)
                    {
                        ((FeaturableEvent)@event).Intro = DocumentService.CreateAutomaticFile(introFile, DateTime.Now, title: "Баннер главной страницы");
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: /ControlPanel/Timeline/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FirstOrDefaultAsync(n => n.Id == id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: /ControlPanel/Timeline/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Event @event = await db.Events.FirstOrDefaultAsync(n => n.Id == id);
            db.Events.Remove(@event);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<ActionResult> Promote(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = await db.Events.FirstOrDefaultAsync(n => n.Id == id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        [HttpPost, ActionName("Promote")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PromoteConfirm(int id, int mode)
        {            
            Event @event = await db.Events.FirstOrDefaultAsync(n => n.Id == id);
            if (@event == null)
            {
                return HttpNotFound();
            }

            switch (mode)
            {
                case 0:
                    await db.Database.ExecuteSqlCommandAsync("UPDATE [dbo].[Events] SET [Discriminator] = 'Event', IsFeatured=NULL, OrdersDisabled = NULL, AccreditationsEnabled = NULL WHERE Id=" + @event.Id);
                    break;
                case 1:
                    await db.Database.ExecuteSqlCommandAsync("UPDATE [dbo].[Events] SET [Discriminator] = 'ExternalEvent', IsFeatured=0, OrdersDisabled = NULL, AccreditationsEnabled = NULL WHERE Id=" + @event.Id);
                    break;
                case 2:
                    await db.Database.ExecuteSqlCommandAsync("UPDATE [dbo].[Events] SET [Discriminator] = 'OfficialEvent', IsFeatured=0, OrdersDisabled = 0, AccreditationsEnabled = 0 WHERE Id=" + @event.Id);
                    break;
            }
            


            return RedirectToAction("Edit", new { id = @event.Id });
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
