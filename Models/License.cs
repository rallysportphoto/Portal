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
    public class License
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int Season { get; set; }
        public string Number { get; set; }
        public DateTime IssuesOn { get; set; }
        public LicenseType Type { get; set; }
    }

    public enum LicenseType
    {
        Driver,
        Marchal
    }
}