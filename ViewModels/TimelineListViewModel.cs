/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class TimelineListViewModel
    {        
        public TimelineListViewModel(Models.Event ev)
        {
            Title = ev.Title;
            Month = new DateTime(ev.StartDate.Year, ev.StartDate.Month, 1);
            if (ev.StartDate.Date == ev.FinishDate.Date)
                Dates = ev.StartDate.Day.ToString();                            
            else
                Dates = ev.StartDate.Day + "-" + ev.FinishDate.Day;
            
            Disciplines = string.Join(", ", ev.Series.Select(s => s.Discipline.Title).Distinct().ToArray());
            Series = string.Join(", ", ev.Series.Select(s => s.ShortName).Distinct().ToArray());
            Place = ev.Place;

            var externalEvent = ev as Models.ExternalEvent;
            if(externalEvent!=null) {
                IsExternal = true;
                Url = externalEvent.Url;
            }

            var officialEvent = ev as Models.OfficialEvent;
            if (officialEvent != null)
            {
                IsOfficial = true;
                Url = "/timeline/" + officialEvent.Slug;
            }
        }

        public string Dates { get; private set; }
        public DateTime Month { get; private set; }
        public string Title { get; private set; }
        public string Series { get; private set; }
        public string Disciplines { get; private set; }

        public bool IsExternal { get; private set; }
        public bool IsOfficial { get; private set; }
        public string Url { get; private set; }

        public object Place { get; private set; }

        
    }
}