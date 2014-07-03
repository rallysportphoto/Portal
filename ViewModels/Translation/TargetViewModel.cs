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
    public class TargetViewModel
    {
        public TargetViewModel(TrackMePro.TargetStageViewModel r)
        {
            CoDriver = r.CoDriver;
            Driver = r.Driver;
            Groups = r.Group;
            Result = TimeSpan.FromSeconds((double)r.Result);
            StartNumber = r.StartNumber;
            StartTime = r.StartTime;
        }

        public DateTime? StartTime { get; set; }

        public int? StartNumber { get; set; }

        public TimeSpan Result { get; set; }

        public string Groups { get; set; }

        public string Driver { get; set; }

        public string CoDriver { get; set; }
    }
}