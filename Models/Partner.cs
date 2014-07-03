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
    public class Partner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desctiption { get; set; }
        public bool Global { get; set; }
        public string Url { get; set; }
        public bool IsEnabled { get; set; }
        public int Position { get; set; }

        public List<Tag> Tags { get; set; }

        public File Logo { get; set; }
    }
}