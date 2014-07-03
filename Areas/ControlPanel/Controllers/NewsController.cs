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
    [Authorize(Roles = "News")]
    public class NewsController : Controller
    {
        private SportDataContext db = new SportDataContext();

        // GET: /ControlPanel/News/
        public async Task<ActionResult> Index()
        {
            return View(await db.News.ToListAsync());
        }
        

        // GET: /ControlPanel/News/Create
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
        public async Task<ActionResult> Create([Bind(Include="Id,Date,Slug,Title,Body,Season,Type")] News news, string[] tags, HttpPostedFileBase[] attachments)
        {
            if (ModelState.IsValid)
            {
                db.News.Add(news);
                if (tags != null)
                {
                    news.Tags = new List<Tag>();
                    var tagList = await db.Tags.ToListAsync();

                    foreach (var tag in tags)
                    {
                        news.Tags.Add(tagList.First(t => t.Id == tag));
                    }
                }
                if (attachments != null)
                {
                    news.Attachments = new List<File>();
                    foreach (var file in attachments)
                    {
                        if (file == null) continue;
                        news.Attachments.Add(DocumentService.CreateAutomaticFile(file,news.Date,title: news.Title));
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(news);
        }

        // GET: /ControlPanel/News/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.Include(n=>n.Tags).FirstOrDefaultAsync(n=>n.Id==id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(news);
        }

        // POST: /ControlPanel/News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Date,Slug,Title,Body,Season,Type")] News news, string[] tags, HttpPostedFileBase[] attachments)
        {
            db.Entry(news).State = EntityState.Modified;
            await db.Entry(news).Collection(n => n.Tags).LoadAsync();
            if (ModelState.IsValid)
            {
                var tagList = await db.Tags.ToListAsync();
                news.Tags.Clear();
                if (tags != null)
                {
                    foreach (var tag in tags)
                    {
                        news.Tags.Add(tagList.First(t => t.Id == tag));
                    }
                }
                if (attachments != null)
                {
                    await db.Entry(news).Collection(n => n.Attachments).LoadAsync();
                    foreach (var file in attachments)
                    {
                        if (file == null) continue;

                        news.Attachments.Add(DocumentService.CreateAutomaticFile(file, news.Date, title: news.Title));                        
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Tags = await db.Tags.ToListAsync();
            return View(news);
        }

        // GET: /ControlPanel/News/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = await db.News.FirstOrDefaultAsync(n => n.Id == id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: /ControlPanel/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            News news = await db.News.FirstOrDefaultAsync(n => n.Id == id);
            db.News.Remove(news);
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
