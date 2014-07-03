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
    public class Gallery
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Author { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime PublicationTime { get; set; }
        public bool IsPublic { get; set; }

        public virtual List<File> Files { get; set; }
        public virtual Tag Tag { get; set; }
    }
}