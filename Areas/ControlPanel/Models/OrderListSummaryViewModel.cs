/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Areas.ControlPanel.Models
{
    public class OrderListSummaryViewModel
    {
        public string EventName { get; set; }
        public string SubEvent { get; set; }
        public int OrdersCount { get; set; }
        public int RedimmedCount { get; set; }
        public int OrdersWithoutNumbersCount { get; set; }
        public string Id { get; set; }
    }
}