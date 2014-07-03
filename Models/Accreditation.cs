/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Accreditation
    {
        public int Id { get; set; } 
        public Tag Tag { get; set; }
        public User User { get; set; }

        public string Car { get; set; }
        public List<MediaInfoEntry> Media { get; set; }

        [DefaultValue(false)]
        public bool Valid { get; set; }

        public int? TimingSystemId { get; set; }
        //TODO материал прошлого года.
    }

    public class MediaInfoEntry {
        public int Id { get; set;}
        public string Media { get;set;}
        public string Position { get; set;}
        public string Organisation {get; set;}
        public string Phone { get; set;}
        public string Fax { get; set;}
        public string Email { get; set;}
        public string Web { get; set;}
        public MediaType Type { get; set; }

        public string Region {get; set;}
        public string Frequency  {get; set;}
        public string Edition {get;set;}
    }
    public enum MediaType
    {
        Print,
        Web,
        TV,
        Photo,
        Team
    }
    
}