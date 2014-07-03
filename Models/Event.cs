/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int Season { get; set; }

        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        
        public List<Series> Series { get; set; }

        public string Place { get; set; }        
    }
    public abstract class FeaturableEvent : Event
    {
        [DefaultValue(false)]
        public bool IsFeatured { get; set; }
        public File Intro { get; set; }
    }

    public class ExternalEvent : FeaturableEvent
    {
        public string Url { get; set; }
    }

    public class OfficialEvent : FeaturableEvent
    {
        public string Slug { get; set; }
        public List<SubEvent> SubEvents { get; set; }                
        public string History { get; set; }

        [DefaultValue(false)]
        public bool AccreditationsEnabled { get; set; }
        [DefaultValue(false)]
        public bool OrdersDisabled { get; set; }

        public string EmergencyPhone { get; set; }
    }

    public class SubEvent
    {
        public int Id { get; set; }
        public OfficialEvent Event { get; set;}
        public Series Series { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }        
        public File Map { get; set; }
        public File Logo { get; set; }        
        public EventTag Tag { get; set; }
        
        public List<Group> Groups { get; set; }

        public int? TimingSystemId { get; set; }
        
        public Guid? SecondaryId { get; set; }
    }
}