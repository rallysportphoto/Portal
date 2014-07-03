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
    public class StageViewModel
    {
        public StageViewModel(TrackMePro.ResultStageViewModel r)
        {
            CoDriver = r.CoDriver;
            Driver = r.Driver;
            Groups = r.Group;
            
            PostStagePosition = r.PositionAfter;
            PreStagePosition = r.PositionBefore;
            if(r.ResultRaw<10000)
                Result = TimeSpan.FromSeconds((double)r.ResultRaw); //TODO recalc  r.IsSecondsResult;
            else
                Result = TimeSpan.FromTicks((long)r.ResultRaw); //TODO recalc  r.IsSecondsResult;
            Penality = TimeSpan.FromSeconds((double)r.Penality);
            OveralResult = TimeSpan.FromSeconds((double)r.ResultAfter);
            HasResult = r.HasResult;
            RetireReason = r.RetireReason;
            StartNumber = r.StartNumber;
        }

        public string CoDriver { get; set; }

        public string Driver { get; set; }

        public string Groups { get; set; }

        public bool HasResult { get; set; }

        public decimal PostStagePosition { get; set; }

        public int PreStagePosition { get; set; }

        public TimeSpan Result { get; set; }
        public TimeSpan OveralResult { get; set; }
        public TimeSpan StageResult { get { return Result + Penality;  } }
        public TimeSpan Penality { get; set; }

        public string RetireReason { get; set; }

        public int? StartNumber { get; set; }
    }
}