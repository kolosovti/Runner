using System;
using System.Collections.Generic;
using Game.Configs;
using Game.Core.Road;
using UniRx;

namespace Game.Core.Model
{
    public interface ILevelModel
    {
        LevelConfig Config { get; }
        IObservable<Unit> LevelSpawned { get; }
        IReadOnlyList<BaseRoadObject> RoadObjectSequence { get; }
    }

    public class LevelModel : ILevelModel
    {
        private LevelConfig _config;
        private Subject<Unit> _levelSpawned = new Subject<Unit>();

        public LevelConfig Config => _config;
        public List<BaseRoadObject> _roadObjectsSequence = new List<BaseRoadObject>();

        LevelConfig ILevelModel.Config => _config;
        IObservable<Unit> ILevelModel.LevelSpawned => _levelSpawned;
        IReadOnlyList<BaseRoadObject> ILevelModel.RoadObjectSequence => _roadObjectsSequence;

        public LevelModel(LevelConfig levelConfig)
        {
            _config = levelConfig;
        }

        public void AddRoadBlockInSequence(BaseRoadObject road)
        {
            _roadObjectsSequence.Add(road);
        }

        public void LevelSpawned()
        {
            _levelSpawned.OnNext(Unit.Default);
        }
    }
}