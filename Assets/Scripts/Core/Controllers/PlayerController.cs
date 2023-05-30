using System;
using Game.Core.Level;
using Game.Core.Model;
using Game.Core.Movement;
using Game.Helpers;
using Game.System;
using Game.UI;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Core.Controllers
{
    public class PlayerController : BaseContextController
    {
        private readonly ILevelLoadingModel _levelLoadingModel;
        private readonly IGameplayModel _gameplayModel;
        private readonly PlayerModel _playerModel;
        private readonly ILevelModel _levelModel;
        private readonly IInputModel _inputModel;

        private IMoveStrategy _playerMoveStrategy;
        private IJumpStrategy _playerJumpStrategy;
        private IDisposable _pathCompleteSubscription;

        private int _nextRoadBlockIndex;

        public PlayerController(
            ILevelLoadingModel levelLoadingModel,
            IGameplayModel gameplayModel,
            PlayerModel playerModel,
            ILevelModel levelModel,
            IInputModel inputModel,
            ContextManager contextManager)
            : base(contextManager)
        {
            _levelLoadingModel = levelLoadingModel;
            _playerModel = playerModel;
            _gameplayModel = gameplayModel;
            _inputModel = inputModel;
            _levelModel = levelModel;

            _playerModel.Health.Value = Services.Configs.PlayerConfig.MaxHealth;
        }

        public override void ConnectController()
        {
            base.ConnectController();

            _inputModel.Jump.Subscribe(x => OnJumpInputReceived()).AddTo(_subscriptions);
            _levelModel.LevelSpawned.First().Subscribe(x => OnLevelSpawned()).AddTo(_subscriptions);
            _gameplayModel.Death.Subscribe(x => SetMockMovementStrategy()).AddTo(_subscriptions);
            _gameplayModel.Revive.Subscribe(x => RevivePlayer()).AddTo(_subscriptions);
            _gameplayModel.Finish.First().Subscribe(x => SetMockMovementStrategy()).AddTo(_subscriptions);
            _levelLoadingModel.LevelLoaded.First().Subscribe(x => OnSceneLoaded()).AddTo(_subscriptions);
        }

        private void OnSceneLoaded()
        {
            var levelActivationLocker = new ReactiveProperty<bool>();
            _levelLoadingModel.RegisterSceneActivationLocker(levelActivationLocker);

            SpawnPlayer();
            //TODO: подумать как грузить ассеты параллельно
            SpawnHealthView();

            levelActivationLocker.Value = true;
        }

        private void OnLevelSpawned()
        {
            TrySetNextPlayerMovementStrategy();
            SubscribeToObstacles();
        }

        private async void SpawnPlayer()
        {
            var assetsController = GetController<AssetsController>();
            await assetsController.LoadPlayerPrefab();

            var playerPrefab = assetsController.PlayerPrefab;
            var player = Object.Instantiate(playerPrefab);

            _playerModel.SetPlayer(player.GetComponent<Player>());
            _playerModel.Player.SetController(this);

            //TODO: extract to camera controller, get from mono-provider without camera.main
            var camera = Camera.main;

            camera.transform.SetParent(_playerModel.Player.transform);
            camera.transform.localPosition = Services.Configs.PlayerCameraConfig.PlayerCameraPosition;
            camera.transform.localRotation = Quaternion.Euler(Services.Configs.PlayerCameraConfig.PlayerCameraRotation);
        }

        private async void SpawnHealthView()
        {
            var assetsController = GetController<AssetsController>();
            await assetsController.LoadHealthViewPrefab();

            var healthViewPrefab = assetsController.HealthViewPrefab;
            var healthView = Object.Instantiate(healthViewPrefab).GetComponent<HealthView>();

            healthView.Set(_playerModel.Health.Value);
            _playerModel.Health.Subscribe(x => healthView.Set(x)).AddTo(_subscriptions);
        }

        private void RevivePlayer()
        {
            _playerModel.Health.Value = Services.Configs.PlayerConfig.MaxHealth;
            _playerModel.Player.Rigidbody.position = new Vector3(_playerModel.Player.Rigidbody.position.x, 0f,
                _playerModel.Player.Rigidbody.position.z);
            _pathCompleteSubscription.Dispose();
            TrySetNextPlayerMovementStrategy();
        }

        private void SetMockMovementStrategy()
        {
            _pathCompleteSubscription.Dispose();

            var strategy = new MockMovementStrategy();
            _playerMoveStrategy = strategy;
            _playerJumpStrategy = strategy;
        }

        private void TrySetNextPlayerMovementStrategy()
        {
            if (_levelModel.RoadObjectSequence.Count - 1 < _nextRoadBlockIndex)
            {
                return;
            }

            var roadBlock = _levelModel.RoadObjectSequence[_nextRoadBlockIndex];

            var pathSettings = new BezierSegmentSettings(
                roadBlock.GetStartPointWorldPosition(),
                roadBlock.GetEndPointWorldSpacePosition(),
                roadBlock.GetStartPointTangent(),
                roadBlock.GetEndPointTangent()
            );

            var movementStrategy = new PlayerMovementStrategy(
                _playerModel,
                _playerModel.Player.Rigidbody,
                Services.Configs.PlayerMovementConfig,
                pathSettings,
                GetPlayerYForce());

            _playerMoveStrategy = movementStrategy;
            _playerJumpStrategy = movementStrategy;

            _pathCompleteSubscription = _playerMoveStrategy.PathComplete.First()
                .Subscribe(x => TrySetNextPlayerMovementStrategy());
            _nextRoadBlockIndex++;
        }

        private void SubscribeToObstacles()
        {
            foreach (var baseRoad in _levelModel.RoadObjectSequence)
            {
                (baseRoad as IObstacleProvider)?.ObstacleEnter.First().Subscribe(x => HandleObstacle(x))
                    .AddTo(_subscriptions);
            }
        }

        private void HandleObstacle(ObstacleType type)
        {
            if (Services.Configs.ObstaclesConfig.TryGetObstacleConfigByType(type, out var config))
            {
                _playerModel.Health.Value -= config.Damage;
            }
        }

        private float GetPlayerYForce()
        {
            if (_playerMoveStrategy != null)
            {
                return (_playerMoveStrategy as PlayerMovementStrategy)?.YForce ?? 0f;
            }

            return 0f;
        }

        public override void FixedTick()
        {
            base.FixedTick();

            //TODO: создать стратегию движения на финише, этот код не очень
            if (_playerMoveStrategy != null)
            {
                _playerMoveStrategy.FixedTick();
            }
        }

        private void OnJumpInputReceived()
        {
            if (_playerModel.JumpsCount.Value < Services.Configs.PlayerConfig.MaxJumpCount)
            {
                _playerJumpStrategy.Jump();
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