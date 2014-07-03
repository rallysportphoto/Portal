/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using Portal.Services;

namespace Portal.Areas.ControlPanel.Controllers
{
    [Authorize(Roles = "Media")]
    public class MediaController : Controller
    {
        private SportDataContext db = new SportDataContext();

        // GET: /ControlPanel/News/
        public async Task<ActionResult> Index()
        {
            return View(await db.Galleries.ToListAsync());
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View();
        }

        // POST: /ControlPanel/News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Create([Bind(Include="Slug,Author,Name,Code,PublicationTime,IsPublic")]Gallery gallery, string tag, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                db.Galleries.Add(gallery);
                if(tag!=null)
                    gallery.Tag = await db.Tags.FirstAsync(t => t.Id == tag);

                if (files != null)
                {
                    gallery.Files = new List<File>();
                    foreach (var file in files)
                    {
                        if (file == null) continue;
                        gallery.Files.Add(DocumentService.CreateAutomaticFile(file, DateTime.Now, hidden:true));
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(gallery);
        }

        // GET: /ControlPanel/News/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = await db.Galleries.Include(n => n.Files).FirstOrDefaultAsync(n => n.Id == id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(gallery);
        }

        // POST: /ControlPanel/News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Slug,Author,Name,Code,PublicationTime,IsPublic")]Gallery gallery, string tag, HttpPostedFileBase[] files)
        {
            db.Entry(gallery).State = EntityState.Modified;
            await db.Entry(gallery).Reference(n => n.Tag).LoadAsync();
            if (ModelState.IsValid)
            {
                if (tag != null) gallery.Tag = db.Tags.First(t => t.Id == tag);
                                              
                if (files != null)
                {
                    await db.Entry(gallery).Collection(n => n.Files).LoadAsync();
                    foreach (var file in files)
                    {
                        if (file == null) continue;

                        gallery.Files.Add(DocumentService.CreateAutomaticFile(file, DateTime.Now,hidden:true));
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(gallery);
        }
	}
}