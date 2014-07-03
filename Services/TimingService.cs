/*

 Copyright (c) 2013-2014 Dmitry Fedorov
 Distributed under the GNU GPL v2. For full terms see the file COPYING.txt

*/
using Portal.Models;
using Portal.ViewModels.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Data.Entity;
using System.Threading;
using Portal.Helpers;

namespace Portal.Services
{
    public class TimingService
    {
        private static Dictionary<int, RaceTimingInfo> races = new Dictionary<int,RaceTimingInfo>();
        private static ReaderWriterLockSlim locker = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        public static async Task Refresh(int id, int? stageId)
        {
            using (var db = new SportDataContext())
            {
                var subEvent = await db.SubEvents.FirstOrDefaultAsync(e => e.TimingSystemId == id);
                if (subEvent == null) return;
                var timingService = new TrackMePro.PortalConnector();                

                var raceInfo = new RaceTimingInfo()
                {
                    EventId = id,
                    StageIds = new Dictionary<string,int>(),
                    Stages = new Dictionary<int,List<StageViewModel>>(),
                    Targets = new Dictionary<int,List<TargetViewModel>>()                    
                };

                var stages = await timingService.StagesAsyncAwaitable(id);                    
                raceInfo.Headers = stages.Select(s => new StageHeaderViewModel(s)).ToList();

                var startingStage = stages.FirstOrDefault(s => s.Id == stageId);
                foreach (var item in stages)
                {
                    raceInfo.StageIds.Add(item.Name, item.Id);
                    if (!item.Name.Contains("КВ"))
                    {
                        var stageResults = await timingService.StageAsyncAwaitable(id, item.Id);
                        raceInfo.Stages.Add(item.Id, stageResults.Select(r => new StageViewModel(r)).ToList());
                    }
                    else
                    {
                        var targetResults = await timingService.TargetAsyncAwaitable(id, item.Id);
                        raceInfo.Targets.Add(item.Id, targetResults.Select(r => new TargetViewModel(r)).ToList());
                    }
                }

                var penalitiesData = timingService.AllPenalities(id);
                raceInfo.Penalities = penalitiesData.Select(p => new PenalityViewModel(p)).ToList();

                var retiresData = timingService.AllRetires(id);
                raceInfo.Retires = retiresData.Select(r => new RetireViewModel(r)).ToList();

                var messagesData = timingService.AllMessages(id);
                raceInfo.Messages = messagesData.Select(m => new MessageViewModel(m)).ToList();

                //locker.EnterWriteLock();
                if (races.ContainsKey(id)) races[id] = raceInfo;
                else races.Add(id, raceInfo);
                //locker.ExitWriteLock();
            }
        }

        private static async Task<T> RefreshIfNeeded<T>(int eventId, Func<RaceTimingInfo, T> action)
        {
            var needRefresh = false;
            RaceTimingInfo race=null;
            locker.EnterReadLock();
                needRefresh = !races.ContainsKey(eventId) ||races[eventId]==null || !races[eventId].Valid;
                if(!needRefresh) race = races[eventId];
            locker.ExitReadLock();
            if(needRefresh) {
                await Refresh(eventId, null);
                locker.EnterReadLock();
                race = races[eventId];
                locker.ExitReadLock();
            }
            return action(race);
        }

        public static async Task<List<StageHeaderViewModel>> GetStageHeaders(int eventId){
            return await RefreshIfNeeded(eventId, r => r.Headers);
        }

        public static async Task<List<StageViewModel>> GetStage(string stageName, int eventId)
        {
            return await RefreshIfNeeded(eventId, r =>
            {
                var stageId = r.StageIds[stageName];
                return r.Stages[stageId];
            });
        }

        public static async Task<List<TargetViewModel>> GetTarget(string stageName, int eventId)
        {
            return await RefreshIfNeeded(eventId, r =>
            {
                var stageId = r.StageIds[stageName];
                return r.Targets[stageId];
            });
        }

        public static async Task<List<PenalityViewModel>> GetPenalities(int eventId)
        {
            return await RefreshIfNeeded(eventId, r => r.Penalities);
        }

        public static async Task<List<RetireViewModel>> GetRetires(int eventId)
        {
            return await RefreshIfNeeded(eventId, r => r.Retires);
        }

        public static async Task<List<MessageViewModel>> GetMessages(int eventId)
        {
            return await RefreshIfNeeded(eventId, r => r.Messages);
        }

        public class RaceTimingInfo
        {
            public int EventId { get; set; }
            public Dictionary<string, int> StageIds { get; set; }
            public List<StageHeaderViewModel> Headers { get; set; }
            public Dictionary<int, List<StageViewModel>> Stages { get; set; }
            public Dictionary<int, List<TargetViewModel>> Targets { get; set; }
            public List<PenalityViewModel> Penalities { get; set; }
            public List<RetireViewModel> Retires { get; set; }

            public bool Valid
            {
                get
                {
                    return StageIds != null &&
                    Headers != null &&
                    Stages != null &&
                    Targets != null &&
                    Penalities != null && Retires != null;
                }
            }

            public List<MessageViewModel> Messages { get; set; }
        }

        internal static object GetGuideSummary(int p)
        {
            throw new NotImplementedException();
        }

        
    }

    public static class TrackMeProAsyncExtentions {
        public static Task<Portal.TrackMePro.ResultMatrixStage[]> StagesAsyncAwaitable(this TrackMePro.PortalConnector client, int stageId)
        {
            var tcs = new TaskCompletionSource<Portal.TrackMePro.ResultMatrixStage[]>();
            client.StagesCompleted += (s, e) => AsyncHelper.TransferCompletion(tcs, e, () => e.Result);
            client.StagesAsync(stageId);
            return tcs.Task;
        }

        public static Task<Portal.TrackMePro.ResultStageViewModel[]> StageAsyncAwaitable(this TrackMePro.PortalConnector client, int eventId, int stageId)
        {
            var tcs = new TaskCompletionSource<Portal.TrackMePro.ResultStageViewModel[]>();
            client.StageCompleted += (s, e) => AsyncHelper.TransferCompletion(tcs, e, () => e.Result);
            client.StageAsync(eventId, stageId);
            return tcs.Task;
        }

        internal static System.Threading.Tasks.Task<Portal.TrackMePro.TargetStageViewModel[]> TargetAsyncAwaitable(this TrackMePro.PortalConnector client, int eventId, int stageId)
        {
            var tcs = new TaskCompletionSource<Portal.TrackMePro.TargetStageViewModel[]>();
            client.TargetCompleted += (s, e) => AsyncHelper.TransferCompletion(tcs, e, () => e.Result);
            client.TargetAsync(eventId,stageId);
            return tcs.Task;
        }

        internal static System.Threading.Tasks.Task<Portal.TrackMePro.PenalityViewModel[]> AllPenalitiesAsyncAwaitable(this TrackMePro.PortalConnector client, int eventId)
        {
            var tcs = new TaskCompletionSource<Portal.TrackMePro.PenalityViewModel[]>();
            client.AllPenalitiesCompleted += (s, e) => AsyncHelper.TransferCompletion(tcs, e, () => e.Result);
            client.AllPenalities(eventId);
            return tcs.Task;
        }

        internal static System.Threading.Tasks.Task<Portal.TrackMePro.RetireViewModel[]> AllRetiresAsyncAwaitable(this TrackMePro.PortalConnector client, int eventId)
        {
            var tcs = new TaskCompletionSource<Portal.TrackMePro.RetireViewModel[]>();
            client.AllRetiresCompleted += (s, e) => AsyncHelper.TransferCompletion(tcs, e, () => e.Result);
            client.AllRetiresAsync(eventId);
            return tcs.Task;
        }
    }
}