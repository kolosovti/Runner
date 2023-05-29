using Game.Core.Model;
using Game.Core.Movement;
using Game.Helpers;
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

        private IMoveStrategy _playerMovementStrategy;

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

            //TODO: extract to camera controller, get from mono-provider without camera.main
            var camera = Camera.main;
            
            camera.transform.SetParent(_playerModel.Player.transform);
            camera.transform.localPosition = Services.Configs.PlayerCameraConfig.PlayerCameraPosition;
            camera.transform.localRotation = Quaternion.Euler(Services.Configs.PlayerCameraConfig.PlayerCameraRotation);

            SetStrategy();
        }

        private void SetStrategy()
        {
            _playerMovementStrategy =
                new BaseMoveStrategy(_playerModel.Player.Rigidbody, Services.Configs.PlayerMovementConfig);
        }

        public override void FixedTick()
        {
            base.FixedTick();
            if (_playerMovementStrategy != null)
            {
                _playerMovementStrategy.FixedTick();
            }
        }

        private void OnJumpInputReceived()
        {
            if (_playerModel.JumpsCount.Value < Services.Configs.PlayerConfig.MaxJumpCount)
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