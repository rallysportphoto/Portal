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
using Portal.Services;
using Portal.ViewModels;

namespace Portal.Controllers
{
    public class MediaController : Controller
    {        
        public async Task<ActionResult> Index(string domain)
        {
            using (var ctx = new SportDataContext())
            {
                //var tags = ctx.Tags
                //    .Include("Galleries").Include("Galleries.Files")
                //    .Where(t => t.Galleries.Where(g => g.IsPublic && g.Files.Any()).Any());                

#warning TODO: фильтрация доменов!
                //var galleries = await tags.SelectMany(t=>t.Galleries).Where(g => g.IsPublic).OrderByDescending(g=>g.PublicationTime).ToListAsync();

                var galleries = await ctx.Galleries.Include(g => g.Files)
                                    .Where(g => g.IsPublic && (g.Files.Any()|| (g.Code!=null && g.Code!=""))).OrderByDescending(g => g.PublicationTime).ToListAsync();

                return View(galleries.Select(g => new GalleryViewModel(g)).ToList());
            }            
        }

        public async Task<ActionResult> Gallery(string slug)
        {
            using (var ctx = new SportDataContext())
            {
                var gallery = await ctx.Galleries.Include(g=>g.Files).FirstOrDefaultAsync(g => g.Slug == slug && g.IsPublic);
                if (gallery == null) return HttpNotFound();

                return View(new GalleryViewModel(gallery));
            }            
        }
	}
}