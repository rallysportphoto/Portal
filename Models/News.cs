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
    public enum NewsType
    {
        Standard,
        Video,
        Quote,
        Gallery
    }

    public class News
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int Season { get; set; }
        public NewsType Type { get; set; }

        public List<Tag> Tags { get; set; }
        public List<File> Attachments { get; set; }
    }
}