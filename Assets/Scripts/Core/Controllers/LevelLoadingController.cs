using Game.Core.Model;
using Game.System;
using UniRx;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Game.Core.Controllers
{
    public class LevelLoadingController : BaseContextController
    {
        private readonly LevelLoadingModel _levelLoadingModel;

        public LevelLoadingController(LevelLoadingModel levelLoadingModel, ContextManager contextManager) : base(
            contextManager)
        {
            _levelLoadingModel = levelLoadingModel;
        }

        protected override void SelfInit()
        {
            base.SelfInit();

            LoadLevel();
            _levelLoadingModel.InitialSpawnComplete.First().Subscribe(x => _levelLoadingModel.ActivateScene())
                .AddTo(_subscriptions);
        }

        private void LoadLevel()
        {
            var handle = GetController<AssetsController>().LoadSceneAsync(_levelLoadingModel.Config.SceneName);
            handle.Completed += OnSceneLoaded;
        }

        private void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _levelLoadingModel.SetLevelLoaded(obj.Result);
            }
            else
            {
                Debug.LogError($"Failed to load scene {_levelLoadingModel.Config.SceneName}");
            }
        }
    }
}