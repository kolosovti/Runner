using Game.Core;
using Game.Core.Controllers;
using Game.Core.Model;
using Game.System;
using Game.UI.Window;
using UniRx;
using UnityEngine;

namespace Game
{
    public class DeathController : BaseContextController
    {
        private readonly IPlayerModel _playerModel;
        private readonly GameplayModel _gameplayModel;

        private DeathWindow _deathWindow;

        public DeathController(IPlayerModel playerModel, GameplayModel gameplayModel, ContextManager contextManager)
            : base(contextManager)
        {
            _playerModel = playerModel;
            _gameplayModel = gameplayModel;
        }

        public override void ConnectController()
        {
            base.ConnectController();

            _playerModel.Health.Subscribe(OnHealthChanged).AddTo(_subscriptions);
            PreloadDeathWindow();
        }

        private async void PreloadDeathWindow()
        {
            await GetController<AssetsController>().LoadDeathWindowPrefab();
        }

        private void OnHealthChanged(float health)
        {
            if (health <= 0)
            {
                _gameplayModel.Death.OnNext(Unit.Default);
                ShowDeathWindow();
            }
        }

        private void ShowDeathWindow()
        {
            var deathWindowPrefab = GetController<AssetsController>().DeathWindowPrefab;
            _deathWindow = Object.Instantiate(deathWindowPrefab).GetComponent<DeathWindow>();
            _deathWindow.Show(GetController<StatisticsController>().GetStatsData());
            _deathWindow.OnReviveButtonClick.First().Subscribe(x => Revive()).AddTo(_subscriptions);
            _deathWindow.OnNextLevelButtonClick.First().Subscribe(x => ReloadLevel()).AddTo(_subscriptions);
        }

        private void Revive()
        {
            _deathWindow.Close();
            _gameplayModel.Revive.OnNext(Unit.Default);
        }

        private void ReloadLevel()
        {
            ContextManager.Dispose();
            GameEntryPoint.Instance.LoadCoreContext();
        }
    }
}
