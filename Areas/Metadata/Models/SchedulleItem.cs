/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Areas.Metadata.Models
{
    public class SchedulleItem
    {
        
        public SchedulleItem(ViewModels.Translation.StageHeaderViewModel s)
        {            
            Name = s.Name;
            Times = s.Comment;
        }
        public string Name { get; set; }
        public string Times { get; set; }
        public int Sort { get; set; }
    }
}