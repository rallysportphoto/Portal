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
using Portal.ViewModels;

namespace Portal.Controllers
{
    public class PartnersController : Controller
    {
        //
        // GET: /Partners/
        public async Task<ActionResult> Public()
        {
            List<Partner> partners;
            using (var ctx = new SportDataContext())
            {
                if (Properties.Settings.Default.DisplayMode == "Event")
                {
                    var ev = await ctx.Events
                    .OfType<OfficialEvent>()
                    .Include("Series")
                    .Include("Series.Tag")
                    .Include("Series.Tag.Partners")
                    .Include("Series.Tag.Partners.Logo")
                    .Include("SubEvents")
                    .Include("SubEvents.Tag")
                    .Include("SubEvents.Tag.Partners")
                    .Include("SubEvents.Tag.Partners.Logo")
                    .FirstOrDefaultAsync(e => e.Slug == "current");

                    partners = ev.SubEvents.SelectMany(s => s.Tag != null ? s.Tag.Partners : new List<Partner>()).ToList();
                    partners = partners.Union(ev.Series.SelectMany(s=>s.Tag!=null? s.Tag.Partners : new List<Partner>())).Distinct().ToList();
                }
                else
                {
                    partners = await ctx.Partners.Where(p => p.IsEnabled && p.Global).ToListAsync();
                }
            }

            return View(partners.Select(p=>new PartnerListViewModel(p)).ToList());
        }
	}
}