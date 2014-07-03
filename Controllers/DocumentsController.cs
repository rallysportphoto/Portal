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
using Portal.ViewModels;
using Portal.Services;

namespace Portal.Controllers
{
    public class DocumentsController : Controller
    {
        //
        // GET: /Documents/
        public ActionResult Index(int season = 2014, string domain="")
        {
            using (var ctx = new SportDataContext())
            {
                var tags = ctx.Tags
                    .Include(c => c.Files)
                    .OfType<DocumentGroup>().Where(t => t.Season == season)
                    .OrderBy(g=>g.Id).ToList()
                    .Select(t=> new DocumentsTagViewModel(t)).AsEnumerable();
                ViewBag.Season = season;
                if (!tags.Any())
                {
                    return View("NoDocuments");
                }                 
                return View(tags);
            }
        }

        [OutputCache(Duration=60*60*24,Location=System.Web.UI.OutputCacheLocation.Downstream)]
        public ActionResult Get(string fileName)
        {            
            #warning TODO: define content-type based on extention            
            var path = Server.MapPath(DocumentService.GetFileRelativePath(fileName));
            if (DocumentService.IsFileExist(path))
                return File(path, "application/octet-stream");
            else
                return HttpNotFound();
        }


	}
}