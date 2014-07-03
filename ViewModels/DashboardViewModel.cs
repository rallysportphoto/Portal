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

namespace Portal.ViewModels
{
    public class DashboardViewModel
    {
        public List<NewsListViewModel> Top3News { get; set; }

        public List<PartnerListViewModel> GlobalPartners { get; set; }
        public List<UpcommingEventViewModel> UpcommingEvents { get; set; }

        public List<FeaturableEventViewModel> FeaturedEvents { get; set; }

        public string Domain { get; set; }

        public class FeaturableEventViewModel
        {
            public FeaturableEventViewModel(FeaturableEvent e)
            {
                var official = e as OfficialEvent;
                var external = e as ExternalEvent;
                Title = e.Title;
                Logo = ThumbnailService.GetThumbnailUrl(e.Intro, 380, 253);
                Year = e.FinishDate.Year;
                Month = e.FinishDate.ToString("MMMM");
                FinishDate = e.FinishDate;

                if (e.StartDate.Day == e.FinishDate.Day)
                    Days = "" + e.FinishDate.Day;
                else
                    Days = e.StartDate.Day + "-" + e.FinishDate.Day;

                if (official != null)
                {
                    Url = "/Timeline/" + official.Slug;
                    Intro = official.History;
                }
                if (external != null)
                {
                    Url = external.Url;
                }
            }
            public DateTime FinishDate { get; set; }
            public string Logo { get; set; }

            public string Intro { get; set; }

            public string Title { get; set; }

            public string Url { get; set; }

            public object Days { get; set; }

            public object Month { get; set; }

            public object Year { get; set; }
        }

        public class UpcommingEventViewModel
        {
            public string Name { get; set; }
            public string Slug { get; set; }            
            public bool AccreditationsEnabled { get; set; }
            public bool OrdersEnabled { get; set; }
            public bool TranslationEnabled { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime FinishDate { get; set; }
            
            public UpcommingEventViewModel(OfficialEvent e)
            {
                Name = e.Title;
                Slug = e.Slug;
                AccreditationsEnabled = e.AccreditationsEnabled;
                OrdersEnabled = e.SubEvents.Any(s => s.Groups.Any()) && e.StartDate>DateTime.Today && !e.OrdersDisabled;
                TranslationEnabled = e.SubEvents.Any(s => s.TimingSystemId != null) && e.StartDate.AddDays(-2)>=DateTime.Today;
                StartDate = e.StartDate;
                FinishDate = e.FinishDate;
            }
        }

        
    }

}