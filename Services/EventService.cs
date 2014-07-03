/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using Portal.Models;
using System.Data.Entity.Infrastructure;

namespace Portal.Services
{
    public class EventService
    {

        internal static async Task<IEnumerable<TimelineListViewModel>> GetSeasonEvents(string domain, int activeSeason, SportDataContext ctx)
        {
            var evQuery = ctx.Events
                .Include(e => e.Series).Include(e => e.Series.Select(s => s.Discipline))
                .Where(ev => ev.Season == activeSeason);

            evQuery = await DomainFilterService.FilterEventsAsync(ctx, domain, evQuery);

            var events = evQuery.OrderBy(e => e.StartDate).ThenBy(e => e.FinishDate).ToList()
                .Select(ev => new TimelineListViewModel(ev));
            return events;
        }


        internal static async Task<OfficialEvent> GetEvent(string slug, SportDataContext db)
        {
            return await db.Events.OfType<OfficialEvent>()
                .Include(e => e.SubEvents)
                .Include(e => e.SubEvents.Select(s => s.Series))
                .Include(e => e.SubEvents.Select(s => s.Map))
                .Include(e => e.SubEvents.Select(s => s.Tag))
                .Include(e => e.SubEvents.Select(s => s.Groups))
                .Include("Series.Tag")
                .Include("Series.Tag.News")
                .Include("Series.Tag.News.Attachments")
                .Include("Series.Tag.Files")
                .Include("Series.Tag.Partners")
                .Include("Series.Tag.Partners.Logo")
                .Include("Series.Tag.Galleries")
                .Include("Series.Tag.Galleries.Files")
                .Include("SubEvents.Tag")
                .Include("SubEvents.Tag.News")
                .Include("SubEvents.Tag.News.Attachments")
                .Include("SubEvents.Tag.Files")
                .Include("SubEvents.Tag.Partners")
                .Include("SubEvents.Tag.Partners.Logo")
                .Include("SubEvents.Tag.Orders")
                .Include("SubEvents.Tag.Orders.Group")
                .Include("SubEvents.Tag.Orders.Car")
                .Include("SubEvents.Tag.Orders.Team")
                .Include("SubEvents.Tag.Orders.Driver")
                .Include("SubEvents.Tag.Orders.CoDriver")
                .Include("SubEvents.Tag.Accreditations")
                .Include("SubEvents.Tag.Accreditations.User")
                .Include("SubEvents.Tag.Accreditations.Media")
                .Include("SubEvents.Tag.Galleries")
                .Include("SubEvents.Tag.Galleries.Files")
                    .FirstOrDefaultAsync(e => e.Slug == slug);
        }
        
        internal static async Task<List<OfficialEvent>> GetUpcommingList(DateTime actualDate, SportDataContext db, string domain)
        {
            var events = db.Events.OfType<OfficialEvent>()
               .Include(e => e.SubEvents).Include("SubEvents.Groups")
               .Include(e=>e.Series)
               .Where(e => e.FinishDate > actualDate && (e.AccreditationsEnabled || e.SubEvents.Any(s => s.TimingSystemId != null || s.Groups.Any())));

            var eventsFiltered = await DomainFilterService.FilterEventsAsync(db,domain,events);

            if (eventsFiltered is IDbAsyncEnumerable)
                return await eventsFiltered.OfType<OfficialEvent>().ToListAsync();
            else
                return eventsFiltered.OfType<OfficialEvent>().ToList();
        }

        internal static IOrderedQueryable<FeaturableEvent> GetFeaturedEvents(SportDataContext db, string domain)
        {
            var items = db.Events.OfType<FeaturableEvent>()
                .Include(e => e.Series)
                .Include(e => e.Intro)
                .Where(e => e.IsFeatured);

            return DomainFilterService.FilterEvents(db, domain, items).OfType<FeaturableEvent>().OrderBy(e => e.FinishDate);
        }
    }
}