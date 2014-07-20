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
    public class Order
    {
        public int Id { get; set; }
        public User Driver { get; set; }
        public User CoDriver { get; set; }
        public Team Team { get; set; }
        public List<Group> Group { get; set; }
        public Car Car { get; set; }

        public bool Confirmed { get; set; }
        public bool Redimmed { get; set; }
        public string StartNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public User CreatedBy { get; set; }

        public int? TimingSystemId { get; set; }

        public string Comments { get; set; }
    }
}