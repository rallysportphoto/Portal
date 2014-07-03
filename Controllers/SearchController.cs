/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Portal.Services;

namespace Portal.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        public ActionResult Team(string name)
        {
            using (var ctx = new SportDataContext())
            {
                var teams = SearchService.FindTeam(name, ctx);                
                return Json(new { found = teams!=null }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<ActionResult> Driver(string license)
        {
            using (var ctx = new SportDataContext())
            {
                var user = await SearchService.FindUser(license, ctx);
                return Json(new { 
                    found = user!=null, 
                    lastName = user!=null? user.LastName : "", 
                    firstName = user!=null? user.FirstName:"" },
                JsonRequestBehavior.AllowGet);
            }
        }        

        public async Task<ActionResult> Me()
        {
            using (var ctx = new SportDataContext())
            {
                var userManager = new UserManager<User>(new UserStore<User>(ctx));
                var user = await userManager.FindByIdAsync(User.Identity.GetUserId());

                await ctx.Entry(user).Collection(u => u.Licenses).LoadAsync();

                var license = user.Licenses.FirstOrDefault(l => l.Season == 2014);
                return Json(new
                {
                    found = user != null,
                    lastName = user != null ? user.LastName : "",
                    firstName = user != null ? user.FirstName : "",
                    license = license!=null? license.Number : "",
                    userId = user!=null ? user.Id : ""
                },
                JsonRequestBehavior.AllowGet);
            }
        }
	}
}