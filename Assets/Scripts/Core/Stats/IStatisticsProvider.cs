using System;
using Game.Core.Controllers;

namespace Game.Core.Stats
{
    public interface IStatisticsProvider
    {
        IObservable<StatisticsContainer> Complete { get; }
    }
}