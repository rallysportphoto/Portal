/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Areas.Metadata.Models
{
    public class RaceViewModel
    {


        public RaceViewModel(Portal.Models.SubEvent s, Portal.Models.OfficialEvent competition)
        {
            Name = competition.Title;
            Logo = "http://www.rallysportphoto.net/file/" + s.Logo.FileName;
            Map = "http://www.rallysportphoto.net/file/" + s.Map.FileName;
            EmergencyPhone = competition.EmergencyPhone;
            Status = s.Title;
            Free = true;
            Id = s.SecondaryId.GetValueOrDefault();            
        }

        public string Name { get; set; }
        public string Logo { get; set; }
        public string Map { get; set; }
        public string EmergencyPhone { get; set; }
        public string CommentPhone { get; set; }
        public List<SchedulleItem> Schedulle { get; set; }
        public List<MessageItem> Messages { get; set; }
        public List<CarResultItem> Results { get; set; }
        public Guid Id { get; set; }
        public string Status { get; set; }
        public bool Free { get; set; }        
    }
}