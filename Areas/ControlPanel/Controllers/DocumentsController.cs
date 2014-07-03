/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Portal.Services;
using System.Threading.Tasks;

namespace Portal.Areas.ControlPanel.Controllers
{
    [Authorize(Roles="Docs")]
    public class DocumentsController : Controller
    {
        private SportDataContext db = new SportDataContext();

        // GET: /ControlPanel/Documents/
        public ActionResult Index()
        {
            return View(db.Files.ToList());
        }
        

        // GET: /ControlPanel/Documents/Create
        public async Task<ActionResult> Create()
        {
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View();
        }

        // POST: /ControlPanel/Documents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title,Description,Date,Hidden")] File file, HttpPostedFileBase content, string[] tags)
        {
            file.FileName = content.FileName;
            var path = Server.MapPath(DocumentService.GetFileRelativePath(file.FileName));

            if(DocumentService.IsFileExist(path)) 
                ModelState.AddModelError("content","Файл с данным именем уже существует");
            if (ModelState.IsValid)
            {
                db.Files.Add(file);
                DocumentService.WriteFile(path, content);
                ThumbnailService.ValidateCache(file);

                file.Tags = new List<Tag>();
                var tagList = await db.Tags.ToListAsync();
                foreach (var tag in tags)
                {
                    file.Tags.Add(tagList.First(t => t.Id == tag));
                }
                await db.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(file);
        }

        // GET: /ControlPanel/Documents/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = await db.Files.Include(f=>f.Tags).FirstOrDefaultAsync(f=>f.Id==id);
            if (file == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(file);
        }

        // POST: /ControlPanel/Documents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FileName,Title,Description,Date")] File file, HttpPostedFileBase content, string[] tags)
        {
            db.Entry(file).State = EntityState.Modified;
            await db.Entry(file).Collection(f => f.Tags).LoadAsync();
            if (ModelState.IsValid)
            {
                var tagList = await db.Tags.ToListAsync();
                file.Tags.Clear();
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        file.Tags.Add(tagList.First(t => t.Id == tag));
                    }
                    await db.SaveChangesAsync();
                }
                
                if (content != null)
                {
                    DocumentService.WriteFile(Server.MapPath(DocumentService.GetFileRelativePath(file.FileName)), content);
                    ThumbnailService.ValidateCache(file);
                }

                return RedirectToAction("Index");
            }            
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(file);
        }

        // GET: /ControlPanel/Documents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            File file = db.Files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: /ControlPanel/Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            File file = db.Files.Find(id);
            db.Files.Remove(file);
            db.SaveChanges();
            var path = Server.MapPath(DocumentService.GetFileRelativePath(file.FileName));
            DocumentService.RemoveFile(path);
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
