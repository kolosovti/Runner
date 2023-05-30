using Game.Core.Model;
using Game.Core.Stats;
using Game.System;
using Game.UI.Window;
using UniRx;

namespace Game.Core.Controllers
{
    public class StatisticsController : BaseContextController
    {
        private readonly ILevelModel _levelModel;
        private readonly IGameplayModel _gameplayModel;
        private readonly StatisticsModel _statisticsModel;

        public StatisticsController(ILevelModel levelModel, IGameplayModel gameplayModel,
            StatisticsModel statisticsModel, ContextManager contextManager) : base(contextManager)
        {
            _levelModel = levelModel;
            _gameplayModel = gameplayModel;
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

        public StatsData GetStatsData()
        {
            var data = new StatsData();
            data.HolePassedCount = GetStatisticsValue(StatsType.HoleComplete);
            data.LongHolePassedCount = GetStatisticsValue(StatsType.LongHoleComplete);
            data.SawPassedCount = GetStatisticsValue(StatsType.SawComplete);
            data.FencePassedCount = GetStatisticsValue(StatsType.FenceComplete);
            return data;
        }

        private float GetStatisticsValue(StatsType type)
        {
            if (_statisticsModel.Statistics.TryGetValue(type, out var value))
            {
                return value;
            }

            return 0f;
        }
    }
}