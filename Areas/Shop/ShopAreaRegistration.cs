/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System.Web.Mvc;

namespace Portal.Areas.Shop
{
    public class ShopAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Shop";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute("Vitrine",
                "Shop/{*uri}",
                new { action = "Index", controller = "Vitrine" });

        }
    }
}