/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Areas.Shop.Models
{
    public class PaymentProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public decimal Discount { get; set; }
        public bool Enabled { get; set; }
    }
}