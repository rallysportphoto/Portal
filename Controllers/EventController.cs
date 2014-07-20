/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Portal.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;

namespace Portal.Controllers
{
    public class EventController : Controller
    {
        //
        // GET: /Event/
        public async Task<ActionResult> Index()
        {
            using (var ctx = new SportDataContext())
            {                
                var ev = await ctx.Events                    
                    .OfType<OfficialEvent>()
                    .Include(e=>e.SubEvents)
                    .Include("SubEvents.Map")
                    .Include("SubEvents.Tag")                    
                    .Include("SubEvents.Tag.Partners")
                    .Include("SubEvents.Tag.Partners.Logo")
                    .FirstOrDefaultAsync(e => e.Slug == "current");

                if (ev == null) return View("CommingSoon");

                return View(new EventDashboardViewModel(ev));
            }
            
        }


        public async Task<ActionResult> Program(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                using (var ctx = new SportDataContext())
                {
                    var ev = await ctx.Events
                    .OfType<OfficialEvent>()
                    .Include(e => e.SubEvents)
                    .Include("SubEvents.Map")
                    .Include("SubEvents.Tag")                                       
                    .FirstOrDefaultAsync(e => e.Slug == "current");
                    if (ev == null) return View("CommingSoon");

                    var model = ev.SubEvents.Select(e => new ProgramItemViewModel(e));
                    return View(model);
                }         
            }
            else
            {
                return await Item(slug);          
            }
        }

        private async Task<ActionResult> Item(string slug)
        {
            using (var ctx = new SportDataContext())
            {
                var ev = await ctx.SubEvents.Include(e=>e.Map).FirstOrDefaultAsync(e => e.Tag.Id == slug);

                if (ev == null) return HttpNotFound();
                
                return View("Item", new ProgramDetailsViewModel(ev));
            }
        }

        
        public async Task<ActionResult> Register()
        {
            using (var db = new SportDataContext())
            {
                var ev = await EventService.GetEvent("current", db);

                if (ev == null)
                    return HttpNotFound();
                                                
                return ev.OrdersDisabled ? View("RegistrationClosed", ev) : View(ev);
            }
        }

        [ActionName("Register"), HttpPost]
        public async Task<ActionResult> RegisterFinish(OrderViewModel modelObj, string slug, HttpPostedFileBase[] attachments)
        {
            if (modelObj == null) modelObj = new OrderViewModel();

            TryUpdateModel(modelObj);

            using (var ctx = new SportDataContext())
            {
                var userManager = new UserManager<User>(new UserStore<User>(ctx));
                //var currentUser = await userManager.FindByIdAsync(User.Identity.GetUserId());

                var team = SearchService.FindTeam(modelObj.Entrant, ctx);
                if (team == null)
                    team = modelObj.CreateTeam();

                var driver1 = await SearchService.FindUser(modelObj.Driver1License, ctx) ?? await SearchService.FindUserById(modelObj.Driver1Id, ctx) ?? await SearchService.FindUserName(modelObj.Driver1FirstName, modelObj.Driver1LastName, ctx);
                var driver2 = await SearchService.FindUser(modelObj.Driver2License, ctx) ?? await SearchService.FindUserById(modelObj.Driver2Id, ctx) ?? await SearchService.FindUserName(modelObj.Driver2FirstName, modelObj.Driver2LastName, ctx);

                //Защита от
                if (driver1 == null || string.IsNullOrWhiteSpace(driver1.LastName + driver1.FirstName + ""))
                {
                    driver1 = modelObj.CreateDriver1();
                    var existingUser = await userManager.FindByNameAsync(driver1.UserName);
                    if (existingUser != null)
                        driver1.UserName = Guid.NewGuid().ToString();
                }
                if (driver2 == null || string.IsNullOrWhiteSpace(driver2.LastName + driver2.FirstName + ""))
                {
                    driver2 = modelObj.CreateDriver2();
                    var existingUser = await userManager.FindByNameAsync(driver2.UserName);
                    if (existingUser != null)
                        driver2.UserName = Guid.NewGuid().ToString();
                }

                var car = modelObj.CreateCar();

                var ev = await EventService.GetEvent("current", ctx);

                Dictionary<SubEvent, Order> orders = new Dictionary<SubEvent, Order>();
                ViewBag.Error = null;
                if (driver1.Id == driver2.Id)
                {
                    ViewBag.Error = "В вашей заявке совпадают первый и второй водитель. Пожалуйста, проверьте правильность заполнения заявки. Если у вас в настоящее время нет полной информации о составе экипажа - оставьте недостающие поля пустыми.";
                }
                if (string.IsNullOrWhiteSpace(driver1.LastName + driver1.FirstName + driver2.LastName + driver2.FirstName))
                {
                    ViewBag.Error = "Укажите пожалуйста в заявке не менее 1 водителя.";
                }
                if (modelObj.Groups == null || modelObj.Groups.Count() == 0)
                {
                    ViewBag.Error = "В вашей заявке на " + ev.Title + " не указано ни одной зачетной группы. Пожалуйста, укажите минимум 1 зачет из списка.";
                }
                if (!string.IsNullOrWhiteSpace(ViewBag.Error))
                    return View("RegistrationError", ev);

                foreach (var group in modelObj.Groups)
                {
                    var tokens = group.Split('_');
                    var subEv = ev.SubEvents.First(e => e.Id == int.Parse(tokens[0]));

                    if (!orders.ContainsKey(subEv))
                    {
                        orders.Add(subEv, new Order()
                        {
                            Driver = driver1,
                            CoDriver = driver2,
                            Team = team,
                            Car = car,
                            Redimmed = false,
                            Confirmed = false,
                            CreatedAt = DateTime.Now,
                            StartNumber = "",
                            Group = new List<Group>(),
                            Comments = modelObj.Comments
                        });
                    }

                    orders[subEv].Group.Add(subEv.Groups.First(g => g.Id == int.Parse(tokens[1])));
                }

                foreach (var item in orders.Keys)
                {

                    item.Tag.Orders = item.Tag.Orders ?? new List<Order>();
                    item.Tag.Orders.Add(orders[item]);
                }
                try
                {
                    await ctx.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }

                if (attachments != null)
                {
                    var iCnt=0;
                    var attachment_template=Server.MapPath("~/App_Data/Attachments/")+ orders.First().Value.Id;
                    foreach (var att in attachments)
                    {
                        if (att != null)
                        {
                            att.SaveAs(attachment_template + '_' + iCnt++ + '.' + Path.GetExtension(att.FileName));
                        }
                    }
                }
                
                return View("RegistrationComplete", ev);
            }
        }
	}
}