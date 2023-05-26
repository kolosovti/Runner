using System.Collections.Generic;
using Game.Core.Model;
using Game.Core.Road;
using Game.Helpers;
using Game.System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Game.Core.Controllers
{
    public class LevelController : BaseContextController
    {
        private readonly ILevelModel _levelModel;

        private SceneInstance _loadedScene;
        private List<AsyncOperationHandle> _loadedResources = new List<AsyncOperationHandle>();

        public LevelController(ILevelModel levelModel, ContextManager contextManager) : base(contextManager)
        {
            _levelModel = levelModel;
        }

        protected override void SelfInit()
        {
            base.SelfInit();
            LoadLevel();
        }

        private void LoadLevel()
        {
            var handle = Addressables.LoadSceneAsync(_levelModel.Config.SceneName);
            handle.AddTo(_loadedResources);
            handle.Completed += OnSceneLoaded;
        }

        private async void OnSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
        {
            if (obj.Status == AsyncOperationStatus.Succeeded)
            {
                _loadedScene = obj.Result;
                var assetsController = GetController<AssetsController>();
                await assetsController.LoadAssetsFromAddressables();
                SpawnLevel();
                _loadedScene.ActivateAsync();
            }
            else
            {
                Debug.LogError($"Failed to load scene {_levelModel.Config.SceneName}");
            }
        }

        private void SpawnLevel()
        {
            var root = new GameObject();
            root.name = "Road root";
            root.transform.position = Vector3.zero;

            var assetsController = GetController<AssetsController>();
            BaseRoadObject previousBlock = null;
            foreach (var roadBlockType in _levelModel.Config.RoadSpawnOrder)
            {
                if (assetsController.TryGetPreloadedRoadPrefabByType(roadBlockType, out var prefab))
                {
                    var roadBlock = Object.Instantiate(prefab, root.transform);
                    if (previousBlock != null)
                    {
                        roadBlock.transform.rotation = previousBlock.transform.rotation * previousBlock.GetEndPointAdditionalRotation();

                        roadBlock.transform.position = previousBlock.GetEndPointWorldSpacePosition() - 
                                                       roadBlock.transform.rotation * roadBlock.GetStartPointLocalPosition();
                    }

                    previousBlock = roadBlock;
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            foreach (var asyncOperationHandle in _loadedResources)
            {
                Addressables.Release(asyncOperationHandle);
            }
        }
    }
}