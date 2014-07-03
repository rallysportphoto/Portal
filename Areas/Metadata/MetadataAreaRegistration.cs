/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System.Web.Mvc;

namespace Portal.Areas.Metadata
{
    public class MetadataAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Metadata";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute("Races.json",
                "Metadata/races.json",
                new { action = "Races", controller = "Races" });

            context.MapRoute("Race_details",
                "Metadata/{id}/{action}.json",
                new { controller = "Races" });
        }
    }
}