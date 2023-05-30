using Game.Core.Model;
using Game.Core.Stats;
using Game.System;
using UniRx;

namespace Game.Core.Controllers
{
    public class StatisticsController : BaseContextController
    {
        private readonly ILevelModel _levelModel;
        private readonly StatisticsModel _statisticsModel;

        public StatisticsController(ILevelModel levelModel, StatisticsModel statisticsModel, 
            ContextManager contextManager) : base(contextManager)
        {
            _levelModel = levelModel;
            _statisticsModel = statisticsModel;
        }

        public override void ConnectController()
        {
            base.ConnectController();

            _levelModel.LevelSpawned.First().Subscribe(x => SubscribeToLevelStats()).AddTo(_subscriptions);
        }

        private void SubscribeToLevelStats()
        {
            foreach (var baseRoadObject in _levelModel.RoadObjectSequence)
            {
                (baseRoadObject as IStatisticsProvider)?.Complete.First().Subscribe(x => HandleStatistics(x))
                    .AddTo(_subscriptions);
            }
        }

        private void HandleStatistics(StatisticsContainer statistics)
        {
            if (_statisticsModel.Statistics.ContainsKey(statistics.Type))
            {
                //TODO: добавить возможность не прибавлять, а оверрайдить стату
                _statisticsModel.Statistics[statistics.Type] += statistics.Amount;
            }
            else
            {
                _statisticsModel.Statistics.Add(statistics.Type, statistics.Amount);
            }
        }
    }
}