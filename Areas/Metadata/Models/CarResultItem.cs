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
    public class CarResultItem
    {
        
        public CarResultItem(Portal.Models.Order o)
        {
            try
            {
                StartNumber = int.Parse(o.StartNumber);
            }
            catch {
                StartNumber = 999;
            }
            if(o.Driver !=null)
                Driver = (o.Driver.LastName ?? "").ToUpper() + " " + o.Driver.FirstName;
            if(o.CoDriver!=null)
                CoDriver= (o.CoDriver.LastName ?? "").ToUpper() + " " + o.CoDriver.FirstName;
            StageResults = new List<StageResult>();
        }
        public int StartNumber { get; set; }
        public string Driver { get; set; }
        public string CoDriver { get; set; }
        public List<StageResult> StageResults { get; set; }
        public string State { get; set; }
    }

    public class StageResult
    {
        public int StageNumber { get; set; }
        public int StartNumber { get; set; }

        public decimal PenalitySeconds { get; set; }
        public decimal ValueSeconds { get; set; }
    }
}