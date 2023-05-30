using System;

namespace Game.Core.Level
{
    public interface IObstacleProvider
    {
        IObservable<ObstacleType> ObstacleEnter { get; }
        IObservable<ObstacleType> ObstacleExit { get; }
    }
}
