/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Areas.Metadata.Models;
using Portal.Models;
using Portal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Portal.Areas.Metadata.Controllers
{
    public class RacesController : Controller
    {
        //
        // GET: /Metadata/Races/        
        public async Task<ActionResult> Races()
        {
            using (var ctx = new SportDataContext())
            {
                var competitions = ctx.Events.OfType<OfficialEvent>()
                    .Include(s=>s.SubEvents)
                    .Include("SubEvents.Map")
                    .Include("SubEvents.Logo")
                    .Include("SubEvents.Tag")
                    .Include("SubEvents.Tag.Orders")
                    .Include("SubEvents.Tag.Orders.Driver")
                    .Include("SubEvents.Tag.Orders.CoDriver")
                    .Where(e => e.SubEvents.Any(se => se.Map != null && se.TimingSystemId != null));

                var model = new List<RaceViewModel>();
                foreach(var competition in competitions) {
                    foreach (var item in competition.SubEvents.Where(s => s.Logo!=null && s.Map != null && s.TimingSystemId.HasValue))
                    {
                        var vm = new RaceViewModel(item, competition);

                        var schedule = await TimingService.GetStageHeaders(item.TimingSystemId.Value);
                        vm.Schedulle = schedule.Where(s => !s.IsTarget).Select(s => new SchedulleItem(s)).ToList();
                        var iCnt = 1;
                        foreach (var i in vm.Schedulle)
                            i.Sort = iCnt++;

                        vm.Messages = await GetMessages(item);
                        
                        vm.Results = item.Tag.Orders.Select(o => new CarResultItem(o)).ToList();

                        var results = await GetAllResults(item);
                        foreach (var entry in vm.Results)                        
                            entry.StageResults = results.Where(r => r.StartNumber == entry.StartNumber).ToList();                        

                        model.Add(vm);
                    }
                }
                return Json(model.ToArray(), JsonRequestBehavior.AllowGet);
            }            
        }

        private static async Task<List<MessageItem>> GetMessages(SubEvent item)
        {
                                var messages = await TimingService.GetMessages(item.TimingSystemId.Value);
                                return messages.Select(m => new MessageItem(m)).ToList();
        }

        public async Task<ActionResult> Entries(Guid id)
        {
            using (var ctx = new SportDataContext())
            {
                var ev = await ctx.SubEvents
                    .Include("Tag")
                    .Include("Tag.Orders")
                    .Include("Tag.Orders.Driver")
                    .Include("Tag.Orders.CoDriver")
                    .FirstAsync(e => e.SecondaryId == id);

                var model = ev.Tag.Orders.Select(o => new CarResultItem(o)).ToList();
                return Json(model.ToArray(), JsonRequestBehavior.AllowGet);                
            }
        }

        public async Task<ActionResult> Messages(Guid id)
        {
            using (var ctx = new SportDataContext())
            {
                var ev = await ctx.SubEvents.FirstAsync(e => e.SecondaryId == id);
                var model = await GetMessages(ev);
                return Json(model.ToArray(), JsonRequestBehavior.AllowGet);
            }            
        }

        public async Task<ActionResult> Stages(Guid id)
        {
            using (var ctx = new SportDataContext())
            {
                var ev = await ctx.SubEvents.FirstAsync(e => e.SecondaryId == id);
                var results = await GetAllResults(ev);
                return Json(results.ToArray(), JsonRequestBehavior.AllowGet);

            }
        }

        private static async Task<List<StageResult>> GetAllResults(SubEvent ev)
        {
            var results = new List<StageResult>();

            var schedule = await TimingService.GetStageHeaders(ev.TimingSystemId.Value);
            var stageNumber = 0;
            foreach (var item in schedule)
            {
                if (item.IsTarget) continue;
                stageNumber++;

                foreach (var r in await TimingService.GetStage(item.Name, ev.TimingSystemId.Value))
                {
                    if (!r.HasResult) continue;
                    results.Add(new StageResult()
                    {
                        StageNumber = stageNumber,
                        StartNumber = r.StartNumber.GetValueOrDefault(999),
                        ValueSeconds = (decimal)r.Result.TotalSeconds,
                        PenalitySeconds = (decimal)r.Penality.TotalSeconds
                    });
                }
            }
            return results;
        }
	}
}