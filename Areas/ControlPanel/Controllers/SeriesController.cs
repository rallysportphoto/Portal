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
    public class SeriesController : Controller
    {
        private SportDataContext db = new SportDataContext();

        // GET: /ControlPanel/Series/
        public async Task<ActionResult> Index()
        {
            return View(await db.Series.Include(s=>s.Discipline).ToListAsync());
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
