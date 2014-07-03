/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portal.Models;
using Portal.Areas.ControlPanel.Models;
using Portal.ViewModels;
using Portal.Services;

namespace Portal.Areas.ControlPanel.Controllers
{
    [Authorize(Roles="Orders")]
    public class OrdersController : Controller
    {
        private SportDataContext db = new SportDataContext();

        // GET: /ControlPanel/Orders/
        public async Task<ActionResult> Index()
        {
            //Выводим сводку: событие 
            // тэг  
            //      количество всего
            //      отменено
            //      без стартовых номеров
            //      Кнопки: назначить стартовые номера, список, изменить, печать списка заявленных, печать списка допущенных
            var items = await db.Tags.OfType<EventTag>()
                    .Include(t=>t.Event)
                    .Include(t=>t.Event.Event)
                    .Include(t=>t.Orders)
                .ToListAsync();

            var model = items.Select(e => new OrderListSummaryViewModel()
            {
                EventName = e.Event.Event.Title,
                SubEvent = e.Event.Title,
                OrdersCount= e.Orders.Count(o=>! o.Redimmed),
                RedimmedCount = e.Orders.Count(o => o.Redimmed),
                OrdersWithoutNumbersCount = e.Orders.Count(o=>string.IsNullOrWhiteSpace(o.StartNumber)),
                Id = e.Id
            });
            return View(model);
        }

        public async Task<ActionResult> List(string id)
        {
            var tag = await db.Tags.OfType<EventTag>()
                .Include(t => t.Orders)
                .Include("Orders.Driver")
                .Include("Orders.Driver.Licenses")
                .Include("Orders.CoDriver")
                .Include("Orders.CoDriver.Licenses")
                .Include("Orders.Team")
                .Include("Orders.Group")
                .Include("Orders.Car")

                .FirstAsync(t => t.Id==id);
            var items = tag.Orders.ToList();

            ViewBag.EventName = tag.Name;
            return View(items);
        }

        public async Task<ActionResult> StartNumbers(string id)
        {
            var tag = await db.Tags.OfType<EventTag>()
               .Include(t => t.Orders)
               .Include("Orders.Driver")
               .Include("Orders.Driver.Licenses")
               .Include("Orders.CoDriver")
               .Include("Orders.CoDriver.Licenses")
               .Include("Orders.Team")
               .Include("Orders.Group")
               .Include("Orders.Car")

               .FirstAsync(t => t.Id == id);

            var items = tag.Orders.ToList();

            ViewBag.EventName = tag.Name;
            return View(items);
        }
        
        public async Task<JsonResult> SetStartNumber(int orderId, string number)
        {
            var order = await db.Orders.FindAsync(orderId);
            order.StartNumber = number;
            await db.SaveChangesAsync();

            return Json(new { success = true });
        }
        

        // GET: /ControlPanel/Orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders                
                .Include("Driver")
                .Include("Driver.Licenses")
                .Include("CoDriver")
                .Include("CoDriver.Licenses")
                .Include("Team")
                .Include("Group")
                .Include("Car")
                .FirstOrDefaultAsync(o=>o.Id==id);
            if (order == null)
            {
                return HttpNotFound();
            }
            var tag = await db.Tags.OfType<EventTag>().Include(t=>t.Event).Include("Event.Groups").FirstAsync(t => t.Orders.Any(o => o.Id == id));
            ViewBag.Groups = tag.Event.Groups.ToList();
            ViewBag.EventName = tag.Name;
            ViewBag.EventId = tag.Id;


            return View(new OrderViewModel(order));
        }

        // POST: /ControlPanel/Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(OrderViewModel order)
        {
            if (ModelState.IsValid)
            {
                var dbOrder = await db.Orders.Include(o => o.Car).Include(o=>o.Group).FirstAsync(o => o.Id == order.Id);
                User driver = await SearchService.FindUser(order.Driver1License, db) ?? await SearchService.FindUserName(order.Driver1FirstName, order.Driver1LastName,db) ?? await SearchService.FindUserById(order.Driver1Id,db);
                User coDriver = await SearchService.FindUser(order.Driver2License, db) ?? await SearchService.FindUserName(order.Driver2FirstName, order.Driver2LastName, db) ?? await SearchService.FindUserById(order.Driver2Id, db);

                var team = SearchService.FindTeam(order.Entrant, db) ?? order.CreateTeam();

                if (driver == null)
                {
                    dbOrder.Driver = order.CreateDriver1();
                }
                else
                {
                    try
                    {
                        driver.BirthDate = DateTime.Parse(order.Driver1BirthDate);
                    }
                    catch
                    {
                        driver.BirthDate = driver.BirthDate;
                    }

                    driver.FirstName = order.Driver1FirstName;
                    driver.LastName = order.Driver1LastName;
                    driver.Location = string.IsNullOrWhiteSpace(order.Driver1City) ? driver.Location : order.Driver1City;
                    driver.Passport = string.IsNullOrWhiteSpace(order.Driver1Passport) ? driver.Passport : order.Driver1Passport;
                    driver.Phone = string.IsNullOrWhiteSpace(order.Driver1Phone) ? driver.Phone : order.Driver1Phone;
                    driver.Address = string.IsNullOrWhiteSpace(order.Driver1Address) ? driver.Address : order.Driver1Address;
                    if (!string.IsNullOrWhiteSpace(order.Driver1License))
                    {
                        driver.Licenses = driver.Licenses ?? new List<License>();
                        if (!driver.Licenses.Any(l => l.Season == 2014 && l.Number == order.Driver1License))
                        {
                            db.Licenses.Add(new License()
                            {
                                IssuesOn = DateTime.Today,
                                Number = order.Driver1License,
                                Season = 2014,
                                Type = LicenseType.Driver,
                                User = driver
                            });
                        }
                    }
                    dbOrder.Driver = driver;
                }

                if (coDriver == null)
                {
                    dbOrder.CoDriver = order.CreateDriver2();
                }
                else
                {
                    try
                    {
                        coDriver.BirthDate = DateTime.Parse(order.Driver2BirthDate);
                    } catch {
                        coDriver.BirthDate = coDriver.BirthDate; 
                    }
                    coDriver.FirstName = order.Driver2FirstName;
                    coDriver.LastName = order.Driver2LastName;
                    coDriver.Location = string.IsNullOrWhiteSpace(order.Driver2City)? coDriver.Location: order.Driver2City;
                    coDriver.Passport = string.IsNullOrWhiteSpace(order.Driver2Passport)? coDriver.Passport : order.Driver2Passport;
                    coDriver.Phone = string.IsNullOrWhiteSpace(order.Driver2Phone)? coDriver.Phone: order.Driver2Phone;
                    coDriver.Address = string.IsNullOrWhiteSpace(order.Driver2Address)? coDriver.Address: order.Driver2Address;
                
                    if (!string.IsNullOrWhiteSpace(order.Driver2License))
                    {
                        coDriver.Licenses = coDriver.Licenses ?? new List<License>();
                        if (!coDriver.Licenses.Any(l => l.Season == 2014 && l.Number == order.Driver2License))
                        {
                            db.Licenses.Add(new License()
                            {
                                IssuesOn = DateTime.Today,
                                Number = order.Driver2License,
                                Season = 2014,
                                Type = LicenseType.Driver,
                                User = coDriver
                            });
                        }
                    }
                    dbOrder.CoDriver = coDriver;
                }                
                // данные по пользователю всегда для текущего (ajax при смене № лицензии)

                team.Address = order.EntrantAddress;
                team.ASN = order.EntrantASN;
                team.City = order.EntrantCity;
                team.Email = order.EntrantEmail;
                team.Fax = order.EntrantFax;
                team.License = order.EntrantLicense;
                team.Phone = order.EntrantPhone;

                dbOrder.Team = team;

                dbOrder.Car.Engine = order.Engine;
                dbOrder.Car.Mark = order.Mark;
                dbOrder.Car.Model = order.Model;
                dbOrder.Car.RegistrationNumber = order.Number;

                dbOrder.Confirmed = true;
                dbOrder.StartNumber = order.StartNumber;
                var tag = await db.Tags.OfType<EventTag>().Include(t => t.Event).Include("Event.Groups").FirstAsync(t => t.Orders.Any(o => o.Id == order.Id));


                dbOrder.Group.Clear();
                dbOrder.Group = tag.Event.Groups.Where(g => order.Groups.Contains(g.Id.ToString())).ToList();
                
                await db.SaveChangesAsync();

                

                return RedirectToAction("List", new { id = tag.Id });
            }
            return View(order);
        }

        // GET: /ControlPanel/Orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: /ControlPanel/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Order order = await db.Orders.FindAsync(id);
            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> GeneratePrintForms(int id)
        {
            var order = await db.Orders
                .Include(o=>o.Car)
                .Include(o=>o.CoDriver)
                .Include(o=>o.CoDriver.Licenses)
                .Include(o=>o.Driver)
                .Include(o=>o.Driver.Licenses)
                .Include(o=>o.Team)
                .Include(o=>o.Group)
                .FirstAsync(o => o.Id == id);
            var tag = await db.Tags.OfType<EventTag>().Include(t => t.Event).Include("Event.Groups").FirstAsync(t => t.Orders.Any(o => o.Id == order.Id));
            
            if (tag.Event.TimingSystemId.HasValue)
            {
                var connector = new TrackMePro.PortalConnector();
                var groups = connector.AvailableGroups(tag.Event.TimingSystemId.Value);

                var serverGroups = groups.Where(g => order.Group.Any(grp => grp.Title == g.Name));

                var serverOrder = new TrackMePro.Entry();
                serverOrder.Groups = serverGroups.ToArray();
                serverOrder.EntryFeeToPay = 0;
                serverOrder.EntryFeeToPaid = 0;
                serverOrder.State = TrackMePro.EntryState.Preliminary;
                try {
                    serverOrder.StartNumber = int.Parse(order.StartNumber);
                }catch {
                    serverOrder.StartNumber = null;
                }

                if (order.Driver != null)
                {
                    serverOrder.FirstName = order.Driver.FirstName;
                    serverOrder.LastName = order.Driver.LastName;
                    serverOrder.BirthDay = order.Driver.BirthDate;
                    serverOrder.Address = order.Driver.Address;
                    serverOrder.Email = order.Driver.Email;
                    serverOrder.City = order.Driver.Location;
                    serverOrder.Phone = order.Driver.Phone;
                   
                    serverOrder.Passport = order.Driver.Passport;

                    if (order.Driver.Licenses.Any(l => l.Season == 2014 && l.Type == LicenseType.Driver))
                    {
                        serverOrder.License = order.Driver.Licenses.OrderByDescending(l => l.IssuesOn).First(l => l.Season == 2014 && l.Type == LicenseType.Driver).Number;
                    }
                }

                if (order.CoDriver != null)
                {
                    serverOrder.CoDriverFirstName = order.CoDriver.FirstName;
                    serverOrder.CoDriverLastName = order.CoDriver.LastName;
                    serverOrder.CoDriverBirthDate = order.CoDriver.BirthDate;
                    serverOrder.CoDriverAddress = order.CoDriver.Address;
                    serverOrder.CoDriverEmail = order.CoDriver.Email;
                    serverOrder.CoDriverCity = order.CoDriver.Location;
                    serverOrder.CoDriverPhone = order.CoDriver.Phone;
                    serverOrder.CoDriverPassport = order.CoDriver.Passport;

                    if (order.CoDriver.Licenses.Any(l => l.Season == 2014 && l.Type == LicenseType.Driver))
                    {
                        serverOrder.CoDriverLicense = order.CoDriver.Licenses.OrderByDescending(l => l.IssuesOn).First(l => l.Season == 2014 && l.Type == LicenseType.Driver).Number;
                    }
                }

                if (order.Car != null)
                {
                    serverOrder.Mark = order.Car.Mark;
                    serverOrder.Model = order.Car.Model;
                    serverOrder.RegistrationNumber = order.Car.RegistrationNumber;
                    serverOrder.Engine = order.Car.Engine;                    
                }

                if (order.Team != null)
                {
                    serverOrder.TeamPhone = order.Team.Phone;
                    serverOrder.Team = order.Team.Name;
                    serverOrder.TeamLicense = order.Team.License;
                    serverOrder.TeamFax = order.Team.Fax;
                    serverOrder.TeamEmail = order.Team.Email;
                    serverOrder.TeamCity = order.Team.City;
                    serverOrder.TeamAddress = order.Team.Address;
                }

                order.TimingSystemId = connector.RegisterOrder(tag.Event.TimingSystemId.Value, order.TimingSystemId, serverOrder);
                await db.SaveChangesAsync();

                return RedirectToAction("DownloadPrintForm", new { id = id});
            }
            return View("TimingNotConfigured");
        }

        public async Task<ActionResult> DownloadPrintForm(int id)
        {
            var order = await db.Orders
                .FirstAsync(o => o.Id == id);
            var tag = await db.Tags.OfType<EventTag>().Include(t => t.Event).Include("Event.Groups").FirstAsync(t => t.Orders.Any(o => o.Id == order.Id));

            if (order.TimingSystemId.HasValue && tag.Event.TimingSystemId.HasValue)
            {
                var connector = new TrackMePro.PortalConnector();

                return File(connector.GetOrderPrintForm(tag.Event.TimingSystemId.Value, order.TimingSystemId.Value), "application/pdf");
            }
            return View("TimingNotConfigured");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
