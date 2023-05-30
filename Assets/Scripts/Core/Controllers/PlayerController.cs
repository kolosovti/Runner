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
        private readonly IFinishModel _finishModel;
        private readonly PlayerModel _playerModel;
        private readonly ILevelModel _levelModel;
        private readonly IInputModel _inputModel;

        private BaseMoveStrategy _playerMovementStrategy;

        public PlayerController(
            ILevelLoadingModel levelLoadingModel,
            IFinishModel finishModel,
            PlayerModel playerModel,
            ILevelModel levelModel,
            IInputModel inputModel,
            ContextManager contextManager)
            : base(contextManager)
        {
            _levelLoadingModel = levelLoadingModel;
            _playerModel = playerModel;
            _finishModel = finishModel;
            _inputModel = inputModel;
            _levelModel = levelModel;
        }

        public override void ConnectController()
        {
            base.ConnectController();

            _levelLoadingModel.LevelLoaded.First().Subscribe(x => OnSceneLoaded()).AddTo(_subscriptions);
            _inputModel.Jump.Subscribe(x => OnJumpInputReceived()).AddTo(_subscriptions);
            _levelModel.LevelSpawned.First().Subscribe(x => OnLevelSpawned()).AddTo(_subscriptions);
        }
        
        private void OnSceneLoaded()
        {
            var levelActivationLocker = new ReactiveProperty<bool>();
            _levelLoadingModel.RegisterSceneActivationLocker(levelActivationLocker);

            SpawnPlayer();

            levelActivationLocker.Value = true;
        }

        private void OnLevelSpawned()
        {
            TrySetNextPlayerMovementStrategy(0);
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
        }

        private void TrySetNextPlayerMovementStrategy(int blockIndex)
        {
            if (_levelModel.RoadObjectSequence.Count - 1 < blockIndex)
            {
                return;
            }

            var roadBlock = _levelModel.RoadObjectSequence[blockIndex];

            var pathSettings = new BezierSegmentSettings(
                roadBlock.GetStartPointWorldPosition(),
                roadBlock.GetEndPointWorldSpacePosition(),
                roadBlock.GetStartPointTangent(),
                roadBlock.GetEndPointTangent()
            );

            _playerMovementStrategy = new BaseMoveStrategy(
                _playerModel,
                _playerModel.Player.Rigidbody,
                Services.Configs.PlayerMovementConfig,
                pathSettings,
                _playerMovementStrategy?.YForce ?? 0f);

            _playerMovementStrategy.PathComplete.First().Subscribe(x => TrySetNextPlayerMovementStrategy(blockIndex + 1)).AddTo(_subscriptions);
        }

        public override void FixedTick()
        {
            base.FixedTick();

            //TODO: создать стратегию движения на финише, этот код не очень
            if (_playerMovementStrategy != null && _finishModel.Finish.Value != true)
            {
                _playerMovementStrategy.FixedTick();
            }
        }

        private void OnJumpInputReceived()
        {
            if (_playerModel.JumpsCount.Value < Services.Configs.PlayerConfig.MaxJumpCount)
            {
                _playerMovementStrategy.Jump();
                _playerModel.OnJump();
            }
        }

        public void PlayerGrounded()
        {
            _playerModel.JumpsCount.Value = 0;
            _playerModel.IsGrounded.Value = true;
        }

        public void PlayerFall()
        {
            _playerModel.IsGrounded.Value = false;
        }
    }
}