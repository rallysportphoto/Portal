/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using Portal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ThumbnailsController : Controller
    {
        //
        // GET: /Thumbnails/
        [OutputCache(Duration = 60 * 60 * 24, Location = System.Web.UI.OutputCacheLocation.Downstream)]
        public ActionResult Generate(int width, int height, string name)
        {
            var path = Server.MapPath(ThumbnailService.GetThumbnailPath(name, width, height));
            if (!DocumentService.IsFileExist(path))
            {
                using (var ctx = new SportDataContext())
                {
                    var file = ctx.Files.FirstOrDefault(f => f.FileName == name);
                    if (file == null)
                    {
                        return HttpNotFound();
                    }
                    else
                    {
                        path = ThumbnailService.GenerateThumbnail(file, width, height);
                    }
                }
            }
            return File(path, "application/octet-stream");
        }
	}
}