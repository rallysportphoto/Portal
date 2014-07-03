/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Series
    {
        public int Id { get; set; }
        public int Season { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public Discipline Discipline { get; set; }
        public List<Event> Events { get; set; }

        public string Slug { get; set; }
        public SeriesTag Tag { get; set; }
    }
}