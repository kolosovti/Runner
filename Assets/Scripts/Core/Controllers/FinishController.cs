using Game.Core;
using Game.Core.Controllers;
using Game.Core.Level;
using Game.Core.Model;
using Game.System;
using Game.UI.Window;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class FinishController : BaseContextController
    {
        private readonly ILevelModel _levelModel;
        private readonly GameplayModel _gameplayModel;

        public FinishController(ILevelModel levelModel, GameplayModel gameplayModel,
            ContextManager contextManager) : base(contextManager)
        {
            _levelModel = levelModel;
            _gameplayModel = gameplayModel;
        }

        public override void ConnectController()
        {
            base.ConnectController();

            _levelModel.LevelSpawned.First().Subscribe(x => SubscribeToLevelStats()).AddTo(_subscriptions);
            _gameplayModel.Finish.First().Subscribe(x => ShowFinishWindow()).AddTo(_subscriptions);

            PreloadWinWindow();
        }

        private void SubscribeToLevelStats()
        {
            foreach (var baseRoadObject in _levelModel.RoadObjectSequence)
            {
                (baseRoadObject as IFinishProvider)?.Finish.First().Subscribe(x => HandleFinish())
                    .AddTo(_subscriptions);
            }
        }

        private async void PreloadWinWindow()
        {
            await GetController<AssetsController>().LoadWinWindowPrefab();
        }

        private void HandleFinish()
        {
            _gameplayModel.Finish.OnNext(Unit.Default);
        }

        private void ShowFinishWindow()
        {
            var winWindowPrefab = GetController<AssetsController>().WinWindowPrefab;
            var winWindow = Object.Instantiate(winWindowPrefab).GetComponent<WinWindow>();
            winWindow.Show(GetController<StatisticsController>().GetStatsData());
            winWindow.OnNextLevelButtonClick.First().Subscribe(x => ReloadLevel()).AddTo(_subscriptions);
        }

        private void ReloadLevel()
        {
            ContextManager.Dispose();
            GameEntryPoint.Instance.LoadCoreContext();
        }
    }
}