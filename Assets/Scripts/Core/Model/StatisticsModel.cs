using System.Collections.Generic;
using Game.Core.Stats;

namespace Game
{
    public interface IStatisticsModel
    {
        IReadOnlyDictionary<StatsType, float> Statistics { get; }
    }

    public class StatisticsModel : IStatisticsModel
    {
        public Dictionary<StatsType, float> Statistics = new Dictionary<StatsType, float>();

        IReadOnlyDictionary<StatsType, float> IStatisticsModel.Statistics => Statistics;
    }
}