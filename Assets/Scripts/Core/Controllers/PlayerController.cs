using Game.Core.Model;
using Game.System;
using UniRx;
using UnityEngine;

namespace Game.Core.Controllers
{
    public class PlayerController : BaseContextController
    {
        private readonly ILevelLoadingModel _levelLoadingModel;
        private readonly PlayerModel _playerModel;
        private readonly IInputModel _inputModel;

        public PlayerController(ILevelLoadingModel levelLoadingModel, IInputModel inputModel,
            PlayerModel playerModel, ContextManager contextManager) : base(contextManager)
        {
            _levelLoadingModel = levelLoadingModel;
            _playerModel = playerModel;
            _inputModel = inputModel;
        }

        public override void ConnectController()
        {
            base.ConnectController();

            _levelLoadingModel.LevelLoaded.First().Subscribe(x => OnSceneLoaded()).AddTo(_subscriptions);
            _inputModel.Jump.Subscribe(x => OnJumpInputReceived()).AddTo(_subscriptions);
        }

        private void OnSceneLoaded()
        {
            var levelActivationLocker = new ReactiveProperty<bool>();
            _levelLoadingModel.RegisterSceneActivationLocker(levelActivationLocker);

            SpawnPlayer();

            levelActivationLocker.Value = true;
        }

        private async void SpawnPlayer()
        {
            var assetsController = GetController<AssetsController>();
            await assetsController.LoadPlayerPrefab();

            var playerPrefab = assetsController.GetPlayerPrefab();
            var player = Object.Instantiate(playerPrefab);

            _playerModel.SetPlayer(player.GetComponent<Player>());
            _playerModel.Player.SetController(this);
        }

        private void OnJumpInputReceived()
        {
            if (_playerModel.JumpsCount.Value < 2)
            {
                _playerModel.Player.Jump();
                _playerModel.OnJump();
            }
        }

        public void PlayerGrounded()
        {
            _playerModel.OnGrounded();
        }
    }
}