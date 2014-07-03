/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Portal.Services
{
    public class DomainFilterService
    {
        internal static IQueryable<News> FilterNews(SportDataContext ctx, string domain, IQueryable<News> newsList)
        {
            domain = domain ?? "";

            var site = ctx.Sites
                            .Include(s => s.Tags)                            
                        .FirstOrDefault(s => s.Domain.ToUpper()==domain.ToUpper());
            if (site != null)
            {
                newsList = newsList.ToList().Where(n => n.Tags.Any(t => site.Tags.Contains(t))).AsQueryable();
            }
            return newsList;
        }

        internal static async Task<IQueryable<Event>> FilterEventsAsync(SportDataContext ctx, string domain, IQueryable<Event> evQuery)
        {
            domain = domain ?? "";

            var site = await ctx.Sites
                            .Include(s => s.Tags)                            
                        .FirstOrDefaultAsync(s => s.Domain.ToUpper() == domain.ToUpper());
            if (site != null)
            {
                var eventsTags = await ctx.Tags.Include("Event").Include("Event.Event").OfType<EventTag>().ToListAsync();
                var seriesTags = await ctx.Tags.OfType<SeriesTag>().Include(t => t.Series).ToListAsync();

                var events = eventsTags.Where(t => site.Tags.Contains(t)).Select(t=>t.Event.Event).ToList();
                var series = seriesTags.Where(t => site.Tags.Contains(t)).Select(t => t.Series).ToList();

                var query =  await evQuery.ToListAsync();
                evQuery = query.Where(e => events.Contains(e) || e.Series.Any(s => series.Contains(s))).AsQueryable(); 
            }

            return evQuery;
        }

        internal static IQueryable<Event> FilterEvents(SportDataContext ctx, string domain, IQueryable<Event> evQuery)
        {
            domain = domain ?? "";

            var site = ctx.Sites
                            .Include(s => s.Tags)
                        .FirstOrDefault(s => s.Domain.ToUpper() == domain.ToUpper());
            if (site != null)
            {
                var eventsTags = ctx.Tags.Include("Event").Include("Event.Event").OfType<EventTag>().ToList();
                var seriesTags = ctx.Tags.OfType<SeriesTag>().Include(t => t.Series).ToList();

                var events = eventsTags.Where(t => site.Tags.Contains(t)).Select(t => t.Event.Event).ToList();
                var series = seriesTags.Where(t => site.Tags.Contains(t)).Select(t => t.Series).ToList();

                var query = evQuery.ToList();
                evQuery = query.Where(e => events.Contains(e) || e.Series.Any(s => series.Contains(s))).AsQueryable();
            }

            return evQuery;
        }
    }
}