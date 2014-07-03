/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels.Translation
{
    public class RetireViewModel
    {
        public RetireViewModel(TrackMePro.RetireViewModel p)
        {
            CoDriver = p.CoDriver;
            Control = p.ControlPointName;
            AdditionalSort = p.ControlPointOrder;
            Driver = p.Driver;
            Groups = p.Groups;
            Reason = p.Reason;
            StartNumber = p.StartNumber;
        }

        public string CoDriver { get; set; }

        public string Control { get; set; }

        public int AdditionalSort { get; set; }

        public string Driver { get; set; }

        public string Groups { get; set; }

        public string Reason { get; set; }

        public int StartNumber { get; set; }
    }
}