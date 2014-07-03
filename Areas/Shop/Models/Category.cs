/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Areas.Shop.Models
{
    public class Category
    {
        public int Id { get; set; }
        public Category Parent { get; set; }
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<File> Attachments { get; set; }
        public List<Product> Products { get; set; }
        public bool Enabled { get; set; }
    }
}