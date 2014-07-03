/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using Portal.Providers;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Threading.Tasks;
using Portal.Services;

namespace Portal.Controllers
{
    public class DashboardController : Controller
    {
        
        public async Task<ActionResult> Public(string domain="")
        {
                var model = new DashboardViewModel();
                model.Top3News = NewsProvider.GetTop3(domain);
                model.GlobalPartners = PartnerProvider.GetGlobal();
                model.Domain = domain;

                var actualDate = DateTime.Today.AddDays(-5);
                using (var db = new SportDataContext())
                {
                    model.UpcommingEvents  = await GetUpcommingEvents(actualDate, db, domain);

                    if (Properties.Settings.Default.ShowFeaturedOnMainPage)
                        model.FeaturedEvents = await GetFeaturedEvents(db, domain);                    
                }

                return View(model);
        }

        private static async Task<List<DashboardViewModel.FeaturableEventViewModel>> GetFeaturedEvents(SportDataContext db, string domain)
        {
            var events = await EventService.GetFeaturedEvents(db, domain).ToListAsync();

            return events.Select(e => new Portal.ViewModels.DashboardViewModel.FeaturableEventViewModel(e)).ToList();
        }

        private static async Task<List<Portal.ViewModels.DashboardViewModel.UpcommingEventViewModel>> GetUpcommingEvents(DateTime actualDate, SportDataContext db, string domain)
        {
            var events = await EventService.GetUpcommingList(actualDate, db, domain);

            return events.Select(e => new Portal.ViewModels.DashboardViewModel.UpcommingEventViewModel(e)).ToList();
        }

        
        #warning Данный метод должен быть синхронным. В противном случае на этапе исполнения masterpage возникает deadlock. Почему- пока не ясно.
        [ChildActionOnly]
        public ActionResult Featured(bool isMenu, string domain="")
        {
            using(var db= new SportDataContext()) {
                var actualDate = DateTime.Today.AddDays(-5); //5 дней после окончания событие продолжает висеть

                var items = EventService.GetFeaturedEvents(db, domain);

                if (isMenu)
                {
                    var model = items.ToList();
                    return PartialView("_FeaturedMenu", model);
                }
                else
                {
                    var model = items.Where(e=>e.FinishDate>=actualDate).ToList();
                    return PartialView("_FeaturedBanner", model);
                }
            }
                       
        }

        [ChildActionOnly]
        public ActionResult Logo(string url, string domain = "")
        {
            using (var db = new SportDataContext())
            {
                var site = db.Sites.FirstOrDefault(s => s.Domain.ToUpper() == domain.ToUpper());

                if (site == null) return Content("<a href='/'><img width='150' src='" + url + "' alt=''></a>");
                else return Content("<a href='http://" + Properties.Settings.Default.BaseDomain + "'><img width='150' src='" + url + "' alt=''></a>");
            }
        }


        [ChildActionOnly]
        public ActionResult HomeTitle(string domain = "")
        {
            using (var db = new SportDataContext())
            {
                var site = db.Sites.FirstOrDefault(s => s.Domain.ToUpper() == domain.ToUpper());

                if (site == null) return Content("<li><a href='/'><i class='fa fa-home'></i> Главная</a></li>");
                else return Content("<li><a href='/'><i class='fa fa-home'></i> "+site.Category + "</a></li>");
            }
        }
        
	}
}