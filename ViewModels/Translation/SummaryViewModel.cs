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
    public class SummaryViewModel
    {
        public string Series { get; set; }
        public List<StageHeaderViewModel> Headers { get; set; }
        public Dictionary<string, bool> HasResults { get; set; }


        public string Slug { get; set; }
    }
}