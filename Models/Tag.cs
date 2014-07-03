/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Tag
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<File> Files { get; set; }
        public List<News> News { get; set; }
        public List<Accreditation> Accreditations { get; set; }
        public List<Partner> Partners { get; set; }
        public List<Site> Sites { get; set; }
        public List<Gallery> Galleries { get; set; }
    }

    public class DocumentGroup : Tag
    {
        public int Season { get; set; } 
    }

    public class SeasonTag : Tag
    {
        public int Year { get; set; }
    }

    public class EventTag : Tag
    {
        [Required]
        public SubEvent Event { get; set; }
        public List<Order> Orders { get; set; }
    }

    public class SeriesTag : Tag
    {
        [Required]
        public Series Series { get; set; }
    }    
    
}