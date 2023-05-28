using System;
using System.Collections.Generic;
using System.Linq;
using Game.Configs;
using UniRx;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Game.Core.Model
{
    public interface ILevelLoadingModel
    {
        IObservable<Unit> LevelLoaded { get; }
        void RegisterSceneActivationLocker(ReactiveProperty<bool> locker);
    }

    public class LevelLoadingModel : ILevelLoadingModel
    {
        public LevelConfig Config { get; }

        private SceneInstance _loadedScene;
        private List<ReactiveProperty<bool>> _sceneActivationLockers = new List<ReactiveProperty<bool>>();

        public IObservable<Unit> InitialSpawnComplete = new Subject<Unit>();

        public LevelLoadingModel(LevelConfig levelConfig)
        {
            Config = levelConfig;
        }

        public Subject<Unit> LevelLoaded = new Subject<Unit>();
        IObservable<Unit> ILevelLoadingModel.LevelLoaded => LevelLoaded.AsUnitObservable();

        void ILevelLoadingModel.RegisterSceneActivationLocker(ReactiveProperty<bool> locker)
        {
            _sceneActivationLockers.Add(locker);
            InitialSpawnComplete = _sceneActivationLockers.CombineLatest().Where(x => x.All(_ => _))
                .Select(_ => Unit.Default);
        }

        public void SetLevelLoaded(SceneInstance loadedLevelScene)
        {
            _loadedScene = loadedLevelScene;
            LevelLoaded.OnNext(Unit.Default);
        }

        public void ActivateScene()
        {
            _loadedScene.ActivateAsync();
        }
    }
}