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
using Portal.Providers;
using System.ServiceModel.Syndication;
using Portal.Services;

namespace Portal.Controllers
{
    public class NewsController : Controller
    {
        //
        // GET: /News/
        public ActionResult Index(int year=2014, string domain="")
        {
            if (year > 2014 || year <= 2009) return HttpNotFound();
            var news = NewsProvider.GetSeason(year, domain);            
            
            return View(news);
        }

        public ActionResult Item(string slug, string domain="")
        {
            var item = NewsProvider.Get(slug, domain);
            if (item != null) return View(item);
            return HttpNotFound();
                        
        }

        public ActionResult Rss(string domain="")
        {
            var news = NewsProvider.GetSeason(2014, domain).OrderByDescending(n=>n.Date).Take(10);

            var feedItems = new List<SyndicationItem>();
            foreach (var news_item in news)
            {
                var item = new SyndicationItem()
                {
                    Title = TextSyndicationContent.CreatePlaintextContent(news_item.Title),
                    PublishDate = new DateTimeOffset(news_item.Date),
                    Summary = TextSyndicationContent.CreateHtmlContent(CombineBriefWithImage(news_item)),
                };
                string url = "http://afspb.org.ru/news/" + news_item.Slug;

                var link = new SyndicationLink(new Uri(url));
                link.Title = "Перейти к новости";
                item.Links.Add(link);
                feedItems.Add(item);
            }

            var feed = new SyndicationFeed(
                    "Новости сайта Автомобильной Федерации Санкт-Петербурга и Ленинградской области",
                    "",
                    new Uri("http://afspb.org.ru/news/Rss"),
                    feedItems);

            return new RssResult()
            {
                Feed = feed
            };
        }

        private string CombineBriefWithImage(NewsListViewModel news_item)
        {
            return "<img src='" + news_item.LogoUrlWide + "'/>" + news_item.Brief;
        }
	}
}