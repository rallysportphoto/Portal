/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Portal.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Portal.Services;
using Portal.ViewModels.Translation;
using System.IO;
using System.Drawing.Imaging;
using System.Text;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Gma.QrCodeNet.Encoding;
using System.Drawing;

namespace Portal.Controllers
{
    public class TimelineController : Controller
    {
        public async Task<ActionResult> Index(string displayPeriod, string domain="", int season=2014)
        {
            var activeSeason = GetActiveSeason(displayPeriod, season);

            using (var ctx = new SportDataContext())
            {
                var events = await EventService.GetSeasonEvents(domain, activeSeason, ctx);

                ViewBag.Season = activeSeason;
                return View(events);
            }
        }


        private static int GetActiveSeason(string displayPeriod, int season)
        {
            var activeSeason = season;
            try
            {
                activeSeason = int.Parse(displayPeriod);
            }
            catch
            {
            }
            return activeSeason;
        }

        public async Task<ActionResult> Get(string slug)
        {
            using (var db = new SportDataContext())
            {
                var ev = await EventService.GetEvent(slug, db);
                if (ev == null)
                    return HttpNotFound();

                return View(new EventViewModel(ev));
            }
        }



        #region Ралли-гид и трансляция
        //[OutputCache(Duration = 60 * 60 * 24, Location = System.Web.UI.OutputCacheLocation.Downstream)]
        public async Task<ActionResult> QR(string slug, int width=8)
        {
            QrEncoder encoder = new QrEncoder(ErrorCorrectionLevel.L);
            
            using (var db = new SportDataContext())
            {
                var ev = await EventService.GetEvent(slug, db);
                if (ev == null)
                    return HttpNotFound();

                var url = "http://afspb.org.ru/timeline/";
                if(ev.Season!=2014) 
                    url += ev.Season + "/";
                url += ev.Slug + "/Guide";
                var qrCode = encoder.Encode(url);

                GraphicsRenderer dRenderer = new GraphicsRenderer(
                        new FixedModuleSize(width, QuietZoneModules.Two),
                        Brushes.Black, Brushes.White);

                using(var ms = new MemoryStream())
                {
                    dRenderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
                    return File(ms.ToArray(), "image/png");
                }
            }

            
        }

        public ActionResult Guide(string slug)
        {
            return RedirectPermanent("/timeline/" + slug);  
        }

        public async Task<ActionResult> Translation (string slug) {
            using (var db = new SportDataContext())
            {
                var competition = await EventService.GetEvent(slug, db);
                if (competition == null) return HttpNotFound();

                var model = new List<SummaryViewModel>();
                foreach (var ev in competition.SubEvents)
                {
                    if (!ev.TimingSystemId.HasValue) continue;
                    
                    var item = new SummaryViewModel() {
                        Slug = ev.Series.Slug,
                        Series = ev.Title,
                        Headers = await TimingService.GetStageHeaders(ev.TimingSystemId.Value),
                        HasResults = new Dictionary<string,bool>()
                    };

                    foreach(var h in item.Headers) {
                        if (!h.IsTarget)
                        {
                            var results = await TimingService.GetStage(h.Name, ev.TimingSystemId.Value);
                            item.HasResults.Add(h.Name, results.Any());
                        }
                        else
                        {
                            var results = await TimingService.GetTarget(h.Name, ev.TimingSystemId.Value);
                            item.HasResults.Add(h.Name, results.Any());
                        }
                    }
                    model.Add(item);
                }

                ViewBag.Event = competition;
                return View(model);

            }
        }

        public async Task<ActionResult> Stage(string slug, string series, string id, string group)
        {
            using (var db = new SportDataContext())
            {
                var competition = await EventService.GetEvent(slug, db);
                if(competition==null) return HttpNotFound();

                var subEvent = competition.SubEvents.FirstOrDefault(e=>e.Series.Slug== series);
                if(subEvent==null|| !subEvent.TimingSystemId.HasValue) return HttpNotFound();

                ViewBag.Headers = await TimingService.GetStageHeaders(subEvent.TimingSystemId.Value);
                ViewBag.Groups = subEvent.Groups.Select(g=>g.Title).ToList();
                ViewBag.Group = group;

                ViewBag.Event = competition;
                ViewBag.Series = subEvent.Series.Title;
                ViewBag.SeriesSlug = series;
                ViewBag.SubEvent = subEvent;

                ViewBag.Title = id;

                var model = await TimingService.GetStage(id,subEvent.TimingSystemId.Value);

                if (!string.IsNullOrWhiteSpace(group)) model = model.Where(m => m.Groups.Contains(group)).ToList();
                return View(model);
            }
        }

        public async Task<ActionResult> Target(string slug, string series, string id, string group)
        {
            using (var db = new SportDataContext())
            {
                var competition = await EventService.GetEvent(slug, db);
                if (competition == null) return HttpNotFound();

                var subEvent = competition.SubEvents.FirstOrDefault(e => e.Series.Slug == series);
                if (subEvent == null || !subEvent.TimingSystemId.HasValue) return HttpNotFound();

                ViewBag.Headers = await TimingService.GetStageHeaders(subEvent.TimingSystemId.Value);
                ViewBag.Groups = subEvent.Groups.Select(g => g.Title).ToList();
                ViewBag.Group = group;

                ViewBag.Event = competition;
                ViewBag.Series = subEvent.Series.Title;
                ViewBag.SeriesSlug = series;
                ViewBag.SubEvent = subEvent;

                ViewBag.Title = id;

                var model = await TimingService.GetTarget(id, subEvent.TimingSystemId.Value);
                if (!string.IsNullOrWhiteSpace(group)) model = model.Where(m => m.Groups.Contains(group)).ToList();
                return View(model);
            }
        }

        public async Task<ActionResult> Penalities(string slug, string series, string group)
        {
            using (var db = new SportDataContext())
            {
                var competition = await EventService.GetEvent(slug, db);
                if (competition == null) return HttpNotFound();

                var subEvent = competition.SubEvents.FirstOrDefault(e => e.Series.Slug == series);
                if (subEvent == null || !subEvent.TimingSystemId.HasValue) return HttpNotFound();

                ViewBag.Headers = await TimingService.GetStageHeaders(subEvent.TimingSystemId.Value);
                ViewBag.Groups = subEvent.Groups.Select(g => g.Title).ToList();                
                ViewBag.Group = group;

                ViewBag.Event = competition;
                ViewBag.Series = subEvent.Series.Title;
                ViewBag.SeriesSlug = series;
                ViewBag.SubEvent = subEvent;

                var model = await TimingService.GetPenalities( subEvent.TimingSystemId.Value);
                if (!string.IsNullOrWhiteSpace(group)) model = model.Where(m => m.Groups.Contains(group)).ToList();
                return View(model);
            }
        }

        public async Task<ActionResult> Retires(string slug, string series, string group)
        {
            using (var db = new SportDataContext())
            {
                var competition = await EventService.GetEvent(slug, db);
                if (competition == null) return HttpNotFound();

                var subEvent = competition.SubEvents.FirstOrDefault(e => e.Series.Slug == series);
                if (subEvent == null || !subEvent.TimingSystemId.HasValue) return HttpNotFound();

                ViewBag.Headers = await TimingService.GetStageHeaders(subEvent.TimingSystemId.Value);
                ViewBag.Groups = subEvent.Groups.Select(g => g.Title).ToList();
                ViewBag.Group = group;

                ViewBag.Event = competition;
                ViewBag.Series = subEvent.Series.Title;
                ViewBag.SeriesSlug = series;
                ViewBag.SubEvent = subEvent;

                var model = await TimingService.GetRetires(subEvent.TimingSystemId.Value);
                if (!string.IsNullOrWhiteSpace(group)) model = model.Where(m => m.Groups.Contains(group)).ToList();
                return View(model);
            }
        }

        public async Task<ActionResult> Comments(string slug, string series, string group)
        {
            using (var db = new SportDataContext())
            {
                var competition = await EventService.GetEvent(slug, db);
                if (competition == null) return HttpNotFound();

                var subEvent = competition.SubEvents.FirstOrDefault(e => e.Series.Slug == series);
                if (subEvent == null || !subEvent.TimingSystemId.HasValue) return HttpNotFound();
                ViewBag.Headers = await TimingService.GetStageHeaders(subEvent.TimingSystemId.Value);
                ViewBag.Groups = subEvent.Groups.Select(g => g.Title).ToList();
                ViewBag.Group = group;

                ViewBag.Event = competition;
                ViewBag.Series = subEvent.Series.Title;
                ViewBag.SeriesSlug = series;
                ViewBag.SubEvent = subEvent;

                var model = await TimingService.GetMessages(subEvent.TimingSystemId.Value);
                return View(model);
            }
        }



        public async Task<ActionResult> Callback(string slug, string token,int id, int? stageId, bool retires = false, bool penalities = false)
        {
            await TimingService.Refresh(id, stageId);
            return Content("OK");
        }

        
        #endregion

        #region Аккредитация
        [Authorize]
        public async Task<ActionResult> Accreditation(string slug)
        {
            using (var db = new SportDataContext())
            {
                var ev = await EventService.GetEvent(slug, db);
                if (ev == null)
                    return HttpNotFound();

                ViewBag.Slug = slug;
                ViewBag.EventName = ev.Title;
                return View();
            }
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AccreditationFinish(Accreditation model, string slug)
        {
            using(var ctx = new SportDataContext()) {
                var userManager = new UserManager<User>(new UserStore<User>(ctx));
                var currentUser = await userManager.FindByIdAsync(User.Identity.GetUserId());

                model.User = currentUser;
                var ev = await EventService.GetEvent(slug, ctx); 

                var tag = ev.SubEvents.Select(s => s.Tag).First();                
                model.Tag = tag;

                tag.Accreditations.Add(model);
                await ctx.SaveChangesAsync();

                ViewBag.Slug = slug;
                ViewBag.EventName = ev.Title;

                EmailService.SendAdministrativeMail("Поступила заявка на аккредитацию на " + ev.Title,
                    @"Поступила заявка на аккредитацию на " + ev.Title + " от " + model.User.LastName + " " + model.User.FirstName + ". Пожалуйста перейдите по ссылке http://www.afspb.org.ru/controlpanel/accreditations/validate/" + model.Id + " для обработки заявки."); 

                return View();
            }
        }
        #endregion

        #region Заявки
        [Authorize]
        public async Task<ActionResult> Register(string slug)
        {
            using (var db = new SportDataContext())
            {
                var ev = await EventService.GetEvent(slug, db);

                if (ev == null)
                    return HttpNotFound();

                

                ViewBag.Slug = slug;
                ViewBag.EventName = ev.Title;
                ViewBag.Driver2Enabled = ev.SubEvents.SelectMany(e => e.Groups).Any(g => g.DriversCount == 2);                
                return ev.OrdersDisabled ? View("RegistrationClosed", ev) : View(ev);
            }
        }

        [Authorize, ActionName("Register"), HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RegisterFinish(OrderViewModel modelObj, string slug)
        {
            if (modelObj == null) modelObj = new OrderViewModel();

            TryUpdateModel(modelObj);

            using (var ctx = new SportDataContext())
            {
                var userManager = new UserManager<User>(new UserStore<User>(ctx));
                var currentUser = await userManager.FindByIdAsync(User.Identity.GetUserId());

                var team = SearchService.FindTeam(modelObj.Entrant, ctx);
                if (team == null)                
                    team = modelObj.CreateTeam();

                var driver1 = await SearchService.FindUser(modelObj.Driver1License, ctx) ?? await SearchService.FindUserById(modelObj.Driver1Id, ctx) ?? await SearchService.FindUserName(modelObj.Driver1FirstName, modelObj.Driver1LastName, ctx);
                var driver2 = await SearchService.FindUser(modelObj.Driver2License, ctx) ?? await SearchService.FindUserById(modelObj.Driver2Id, ctx) ?? await SearchService.FindUserName(modelObj.Driver2FirstName, modelObj.Driver2LastName, ctx);

                //Защита от
                if (driver1==null || string.IsNullOrWhiteSpace(driver1.LastName + driver1.FirstName + "")) {
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

                var ev = await EventService.GetEvent(slug, ctx);

                Dictionary<SubEvent, Order> orders = new Dictionary<SubEvent,Order>();
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
                if(!string.IsNullOrWhiteSpace(ViewBag.Error))
                    return View("RegistrationError", ev);

                foreach (var group in modelObj.Groups)
                {
                    var tokens = group.Split('_');
                    var subEv = ev.SubEvents.First(e=>e.Id == int.Parse(tokens[0]));

                    if(!orders.ContainsKey(subEv)) {
                        orders.Add(subEv, new Order() {
                            Driver = driver1,
                            CoDriver = driver2,
                            Team = team,
                            Car = car,
                            Redimmed = false,
                            Confirmed = false,
                            CreatedAt= DateTime.Now,
                            StartNumber="",
                            Group = new List<Group>(),
                            CreatedBy = currentUser
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

                return View("RegistrationComplete", ev);
            }
        }

        public async Task<ActionResult> Orders(string slug, string series, string group)
        {
            using (var db = new SportDataContext())
            {
                var ev = await EventService.GetEvent(slug, db);
                if (ev == null)
                    return HttpNotFound();

                var subEvent = ev.SubEvents.FirstOrDefault(e => e.Series.Slug == series && e.TimingSystemId.HasValue);
                var model = subEvent.Tag.Orders.Select(o => new OrderListViewModel(o)).ToList();

                ViewBag.Headers = await TimingService.GetStageHeaders(subEvent.TimingSystemId.Value);
                ViewBag.Groups = subEvent.Groups.Select(g => g.Title).ToList();
                ViewBag.Group = group;

                ViewBag.Event = ev;
                ViewBag.Series = subEvent.Series.Title;
                ViewBag.SeriesSlug = series;
                ViewBag.SubEvent = subEvent;

                if (!string.IsNullOrWhiteSpace(group))
                    model = model.Where(o => o.Group.Contains(group)).ToList();

                return View(model);
            }
        }
        #endregion

        #region Заявки на судейство
        [Authorize]
        public async Task<ActionResult> MarchalRequest(string slug)
        {
            Event ev;
            if (slug == "f1")
            {
                ev = new Event()
                {
                    Title = "Формула 1: Гран-При Россия",
                    StartDate = new DateTime(2014, 10, 10),
                    FinishDate = new DateTime(2014, 10, 12),
                    Place = "Сочи",
                    Season = 2014
                };
                //hardcode для F1
            }
            else
            {
                using (var db = new SportDataContext())
                {
                    ev = await db.Events
                        .OfType<OfficialEvent>()                        
                        .FirstOrDefaultAsync(e => e.Slug == slug);

                    if (ev == null)
                        return HttpNotFound();
                }                
            }
            ViewBag.Slug = slug;
            ViewBag.EventName = ev.Title;
            return View(ev);

        }

        [Authorize, ActionName("MarchalRequest"), HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MarchalRequestFinish(MarchalRequestViewModel modelObj, string slug)
        {
            throw new NotImplementedException();
            //if (modelObj == null) modelObj = new OrderViewModel();

            //TryUpdateModel(modelObj);

            //using (var ctx = new SportDataContext())
            //{
            //    var userManager = new UserManager<User>(new UserStore<User>(ctx));
            //    var currentUser = await userManager.FindByIdAsync(User.Identity.GetUserId());

            //    var team = SearchService.FindTeam(modelObj.Entrant, ctx);
            //    if (team == null)
            //        team = modelObj.CreateTeam();

            //    var driver1 = await SearchService.FindUser(modelObj.Driver1License, ctx) ?? await SearchService.FindUserById(modelObj.Driver1Id, ctx) ?? await SearchService.FindUserName(modelObj.Driver1FirstName, modelObj.Driver1LastName, ctx) ?? modelObj.CreateDriver1();
            //    var driver2 = await SearchService.FindUser(modelObj.Driver2License, ctx) ?? await SearchService.FindUserById(modelObj.Driver2Id, ctx) ?? await SearchService.FindUserName(modelObj.Driver2FirstName, modelObj.Driver2LastName, ctx) ?? modelObj.CreateDriver2();

            //    var car = modelObj.CreateCar();

            //    var ev = await ctx.Events
            //        .OfType<OfficialEvent>()
            //        .Include("SubEvents")
            //        .Include("SubEvents.Groups")
            //        .Include("SubEvents.Series")
            //        .Include("SubEvents.Tag")
            //        .FirstOrDefaultAsync(e => e.Slug == slug);

            //    Dictionary<SubEvent, Order> orders = new Dictionary<SubEvent, Order>();
            //    if (modelObj.Groups == null || modelObj.Groups.Count() == 0) return View("GroupNotDefined", ev);
            //    foreach (var group in modelObj.Groups)
            //    {
            //        var tokens = group.Split('_');
            //        var subEv = ev.SubEvents.First(e => e.Id == int.Parse(tokens[0]));

            //        if (!orders.ContainsKey(subEv))
            //        {
            //            orders.Add(subEv, new Order()
            //            {
            //                Driver = driver1,
            //                CoDriver = driver2,
            //                Team = team,
            //                Car = car,
            //                Redimmed = false,
            //                Confirmed = false,
            //                CreatedAt = DateTime.Now,
            //                StartNumber = "",
            //                Group = new List<Group>(),
            //                CreatedBy = currentUser
            //            });
            //        }

            //        orders[subEv].Group.Add(subEv.Groups.First(g => g.Id == int.Parse(tokens[1])));
            //    }

            //    foreach (var item in orders.Keys)
            //    {
            //        item.Tag.Orders = new List<Order>();
            //        item.Tag.Orders.Add(orders[item]);
            //    }
            //    try
            //    {
            //        await ctx.SaveChangesAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        throw;
            //    }

            //    return View("RegistrationComplete", ev);
        }        
        #endregion

        #region Legacy
        public ActionResult Details(string slug)
        {
            var fallback = new FallbackController();
            return fallback.Proxy("/timeline/Details/" + slug);
        }

            public ActionResult Partner(string slug)
        {
            var fallback = new FallbackController();
            return fallback.Proxy("/timeline/Partner/" + slug);
        }
        #endregion
    }
}

