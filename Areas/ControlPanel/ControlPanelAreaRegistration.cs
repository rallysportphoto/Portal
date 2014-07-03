/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System.Web.Mvc;

namespace Portal.Areas.ControlPanel
{
    public class ControlPanelAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ControlPanel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ControlPanel_default",
                "controlpanel",
                new { action = "Index", controller = "Dashboard" },
                new[] { "Portal.Areas.ControlPanel.Controllers" }
            );

            context.MapRoute(
                "ControlPanel_all",
                "controlpanel/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "Portal.Areas.ControlPanel.Controllers" }
            );
        }
    }
}