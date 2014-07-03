/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using Portal.Services;

namespace Portal.Areas.ControlPanel.Controllers
{
    public partial class TimelineController
    {
        public async Task<ActionResult> CreateSubEvent(int eventId)
        {

            ViewBag.EventId = eventId;
            Event @event = await db.Events.Include(e => e.Series).FirstAsync(e => e.Id == eventId);

            ViewBag.Series = @event.Series.Select(s => new SelectListItem()
            {
                Text = s.Title,
                Value = s.Id.ToString()
            }).ToList();
            ViewBag.Groups = await db.Groups.ToListAsync();
            return View();
        }

        [HttpPost, ActionName("CreateSubEvent"), ValidateInput(false)]
        [ValidateAntiForgeryToken]        
        public async Task<ActionResult> CreateSubEventConfirmed([Bind(Exclude = "Groups, Id")]SubEvent model, int eventId, int seriesValue, HttpPostedFileBase mapFile, int[] groups)
        {
            ViewBag.EventId = eventId;
            OfficialEvent @event = await db.Events.OfType<OfficialEvent>()
                .Include(e => e.SubEvents).Include(e => e.Series)
                .FirstAsync(e => e.Id == eventId);
            ViewBag.Groups = await db.Groups.ToListAsync();

            if (ModelState.IsValid)
            {
                model.Series = @event.Series.First(s => s.Id == seriesValue);
                model.SecondaryId = Guid.NewGuid();
                @event.SubEvents.Add(model);
                await db.SaveChangesAsync();

                model.Tag = new EventTag()
                {
                    Id = @event.Slug + "_" + model.Series.Slug,
                    Event = model,
                    Files = new List<File>(),
                    Name = @event.Title + " (" + model.Series.Title + ")"
                };

                if (groups != null)
                    model.Groups = ((List<Group>)ViewBag.Groups).Where(g => groups.Contains(g.Id)).ToList();

                if (mapFile != null)
                {
                    var file = DocumentService.CreateAutomaticFile(mapFile, title: "Карта трассы " + model.Event.Title + " зачет " + model.Series.Title);
                    model.Tag.Files.Add(file);
                    model.Map = file;
                }

                await db.SaveChangesAsync();

                return RedirectToAction("Edit", new { id = eventId });
            }

            ViewBag.Series = @event.Series.Select(s => new SelectListItem()
            {
                Text = s.Title,
                Value = s.Id.ToString()
            }).ToList();
            return View(model);
        }

        public async Task<ActionResult> DeleteSubEvent(int id)
        {
            var ev = await db.SubEvents.FirstAsync(e => e.Id == id);
            return View(ev);
        }

        [HttpPost, ActionName("DeleteSubEvent")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteSubEventConfirmed(int id)
        {
            var @event = await db.SubEvents.Include(e => e.Event).FirstAsync(n => n.Id == id);
            var eventId = @event.Event.Id;
            db.SubEvents.Remove(@event);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = eventId });
        }


        public async Task<ActionResult> EditSubEvent(int id)
        {

            var @event = await db.SubEvents
                .Include(e => e.Event)
                .Include(e => e.Event.Series)
                .Include(e => e.Series)
                .Include(e => e.Tag)
                .Include(e => e.Groups)
                .FirstAsync(e => e.Id == id);

            ViewBag.Series = @event.Event.Series.Select(s => new SelectListItem()
            {
                Text = s.Title,
                Value = s.Id.ToString()
            }).ToList();
            ViewBag.Groups = await db.Groups.ToListAsync();
            return View(@event);
        }
        [HttpPost, ActionName("EditSubEvent"), ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditSubEventConfirmed([Bind(Exclude = "Groups")]SubEvent model, int seriesValue, HttpPostedFileBase mapFile, int[] groups)
        {
            db.Entry(model).State = EntityState.Modified;
            await db.Entry(model).Reference(e => e.Event).LoadAsync();
            await db.Entry(model.Event).Collection(e => e.Series).LoadAsync();
            await db.Entry(model).Reference(e => e.Series).LoadAsync();


            model.Series = model.Event.Series.First(s => s.Id == seriesValue);
            ViewBag.Groups = await db.Groups.ToListAsync();

            if (ModelState.IsValid)
            {
                if (groups != null)
                {
                    await db.Entry(model).Collection(e => e.Groups).LoadAsync();
                    model.Groups = ((List<Group>)ViewBag.Groups).Where(g => groups.Contains(g.Id)).ToList();
                }

                await db.SaveChangesAsync();

                if (mapFile != null)
                {
                    await db.Entry(model).Reference(e => e.Map).LoadAsync();
                    if (model.Map == null)
                    {
                        var file = DocumentService.CreateAutomaticFile(mapFile, title: "Карта трассы " + model.Event.Title + " зачет " + model.Series.Title);
                        await db.Entry(model).Reference(e => e.Tag).LoadAsync();
                        await db.Entry(model.Tag).Collection(t=>t.Files).LoadAsync();
                        model.Tag.Files.Add(file);
                        model.Map = file;
                        await db.SaveChangesAsync();
                    }
                    else
                    {
                        DocumentService.WriteFile(Server.MapPath(DocumentService.GetFileRelativePath(model.Map.FileName)), mapFile);
                    }
                }
                return RedirectToAction("Edit", new { id = model.Event.Id });
            }

            ViewBag.Series = model.Event.Series.Select(s => new SelectListItem()
            {
                Text = s.Title,
                Value = s.Id.ToString()
            }).ToList();
            return View(model);
        }
    }
}