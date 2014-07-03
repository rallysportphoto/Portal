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
using Portal.Helpers;

namespace Portal.ViewModels
{
    public class EventViewModel
    {        
        public EventViewModel(Models.OfficialEvent ev)
        {
            Slug = ev.Slug;
            Season = ev.Season;
            Title = ev.Title;
            if (ev.StartDate.Date == ev.FinishDate.Date)
            {
                Dates = ev.StartDate.ToString("dd MMMM yyyy");
            }
            else
            {
                Dates = ev.StartDate.ToString("dd MMMM") + " - " + ev.FinishDate.ToString("dd MMMM yyyy");
            }

            Description = ev.History;
            Series = ev.SubEvents.OrderBy(s=>s.Id).Select(s => new EventSeriesViewModel(s));
            IsOverdue = ev.FinishDate < DateTime.Now;
            AccreditationsEnabled = ev.AccreditationsEnabled;
            RegistrationEnabled = ev.SubEvents.SelectMany(s => s.Groups).Any() && !ev.OrdersDisabled;
        }


        public string Slug { get; set; }

        public int Season { get; set; }

        public string Title { get; set; }

        public string Dates { get; set; }

        public string Description { get; set; }

        public IEnumerable<EventSeriesViewModel> Series { get; set; }

        public bool IsOverdue { get; set; }

        public bool AccreditationsEnabled { get; set; }
        public bool RegistrationEnabled { get; set; }
    }

    public class EventSeriesViewModel
    {        
        public EventSeriesViewModel(Models.SubEvent s)
        {
            SeriesName = s.Series.Title;
            Title = s.Title;
            if(s.Map!=null) {
                MapUrl = ThumbnailService.GetRawFile(s.Map);
                MapThumbnailUrl710 = ThumbnailService.GetThumbnailUrl(s.Map, 710, 710);               
            }
            Description = s.Description;
            Slug = s.Series.Slug;


            News = TagsSummary(s, t => t.News.Select(n => new NewsListViewModel(n)));
            Documents = TagsSummary(s,t => t.Files.Where(f=>!f.Hidden).Select(f => new FileViewModel(f))); 
            Orders = s.Tag.Orders.Select(o => new OrderListViewModel(o)).ToList();
            Accreditations = s.Tag.Accreditations.Select(a => new AccreditationListViewModel(a)).ToList();
            Partners = TagsSummary(s, t => t.Partners.Where(p => p.IsEnabled).Select(p => new PartnerListViewModel(p)));
            Galleries = TagsSummary(s, t => t.Galleries.Where(p => p.IsPublic).Select(g => new GalleryViewModel(g)));

        }

        private static List<T> TagsSummary<T>(Models.SubEvent s, Func<Tag,IEnumerable<T>> convertor)
        {
            var tags = new List<Tag>();
            if (s.Series != null && s.Series.Tag != null) tags.Add(s.Series.Tag);
            if (s.Tag != null) tags.Add(s.Tag);
            
            return tags.SelectMany(convertor).Distinct().ToList();
        }


        public object SeriesName { get; set; }

        public object Title { get; set; }

        public string MapUrl { get; set; }

        public string MapThumbnailUrl710 { get; set; }

        public object Description { get; set; }

        public object Slug { get; set; }

        public List<NewsListViewModel> News { get; set; }
        public List<FileViewModel> Documents { get; set; }
        public List<OrderListViewModel> Orders { get; set; }
        public List<AccreditationListViewModel> Accreditations { get; set; }
        public List<PartnerListViewModel> Partners { get; set; }
        public List<GalleryViewModel> Galleries { get; set; }
    }

    public class AccreditationListViewModel
    {       
        public AccreditationListViewModel(Models.Accreditation a)
        {
            Name = (a.User.LastName??"").ToUpper() + " " + a.User.FirstName;
            Organisation = string.Join(",", a.Media.Select(m => m.Media));
        }

        public string Name { get; set; }
        public string Organisation { get; set; }
    }

    public class OrderListViewModel
    {        
        public OrderListViewModel(Models.Order o)
        {
            StartNumber = o.StartNumber;
            try
            {
                StartNumberInt = int.Parse(o.StartNumber);
            }
            catch
            {
                StartNumberInt = int.MaxValue;
            }

            Driver = o.Driver.LastName.OrDefault().ToUpper() + " " +  o.Driver.FirstName.OrDefault();
            CoDriver = o.CoDriver.LastName.OrDefault().ToUpper() +  " "+ o.CoDriver.FirstName.OrDefault();
            DriverCity = o.Driver.Location.OrDefault();
            CoDriverCity = o.CoDriver.Location.OrDefault();
            Team = o.Team.Name.OrDefault();
            Car = o.Car.Mark.OrDefault() + " " + o.Car.Model.OrDefault();
            Group = string.Join(",", o.Group.Select(s => s.Title.OrDefault()));
        }
        public string StartNumber { get; set; }
        public string Driver { get; set; }
        public string CoDriver { get; set; }
        public string DriverCity { get; set; }
        public string CoDriverCity { get; set; }
        public string Team { get; set; }
        public string Car { get; set; }
        public string Group { get; set; }

        public int StartNumberInt { get; set; }
    }
}