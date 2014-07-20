using Portal.Models;
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
    public class EventDashboardViewModel
    {
         public EventDashboardViewModel(OfficialEvent ev)
        {
            Description = ev.History;
            Partners = ev.SubEvents.SelectMany(e => e.Tag.Partners).Distinct().OrderBy(p => p.Position).Select(p => new PartnerListViewModel(p)).ToList();
            Program = ev.SubEvents.Select(e => new ProgramItemViewModel(e)).ToList();
        }

         public string Description { get; set; }

         public IEnumerable<PartnerListViewModel> Partners { get; set; }

         public IEnumerable<ProgramItemViewModel> Program { get; set; }
    }    
}