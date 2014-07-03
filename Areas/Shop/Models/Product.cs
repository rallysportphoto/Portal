/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Areas.Shop.Models
{
    public class Product
    {
        [Key]
        public string SKU { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<File> Attachments { get; set; }

        public decimal Price { get; set; }
        public decimal Cost { get; set; }
        public int StockCount { get; set; }
        public bool Enabled { get; set; }
        public bool Featured { get; set; }
    }
}