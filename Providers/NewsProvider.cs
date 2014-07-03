/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Portal.Services;

namespace Portal.Providers
{
    public class NewsProvider
    {
        internal static List<ViewModels.NewsListViewModel> GetTop3(string domain)
        {
            using (var ctx = new SportDataContext())
            {
                return 
                    NewsQueryable(ctx, domain).OrderByDescending(n => n.Date).Take(3).ToList().Select(n => new NewsListViewModel(n)).ToList();
            }
        }

        internal static IEnumerable<NewsListViewModel> GetSeason(int year, string domain)
        {
            using (var ctx = new SportDataContext())
            {
                return NewsQueryable(ctx, domain)
                    .Where(n => n.Season == year).OrderByDescending(n => n.Date).ToList().Select(n => new NewsListViewModel(n));
            }
        }

        private static IQueryable<News> NewsQueryable(SportDataContext ctx, string domain)
        {
            var newsList = ctx.News
                                .Include(n => n.Attachments)
                                .Include(n => n.Tags);

            newsList = DomainFilterService.FilterNews(ctx, domain, newsList);

            return newsList;
        }

        

        internal static NewsDetailsViewModel Get(string slug, string domain)
        {
            using (var ctx = new SportDataContext())
            {
                var item = NewsQueryable(ctx, domain).FirstOrDefault(n => n.Slug == slug);
                if (item == null) return null;
                return new NewsDetailsViewModel(item);
            }
        }

        
    }
}