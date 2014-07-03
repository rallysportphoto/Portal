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
    public class StageHeaderViewModel
    {
        
        public StageHeaderViewModel(TrackMePro.ResultMatrixStage s)
        {
            Name = s.Name;
            IsTarget = s.Name.Contains("КВ");
            Comment = s.Description;
        }

        public bool IsTarget { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
    }
}