using System.Collections.Generic;
using Game.Core.Model;
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
            var assetsController = GetController<AssetsController>();
            foreach (var roadBlockType in _levelModel.Config.RoadSpawnOrder)
            {
                if (assetsController.TryGetPreloadedRoadPrefabByType(roadBlockType, out var prefab))
                {
                    Object.Instantiate(prefab);
                }
            }
        }

        /*
        private async Task<List<T>> LoadRoadPrefabsFromAddressables<T>(List<AssetReference> assets)
        {
            var roadBlocks = new List<T>();
            // You can add a typeof() at the end to filter on type as well. Take not of the merge mode here
            AsyncOperationHandle<IList<IResourceLocation>> locationsHandle = Addressables.LoadResourceLocationsAsync(assets, Addressables.MergeMode.Union);
            await locationsHandle.Task;

            var handle = Addressables.LoadAssetsAsync<GameObject>(locationsHandle.Result, x =>
            {
                roadBlocks.Add(x.GetComponent<T>());
            });
            handle.AddTo(_loadedResources);
            await handle.Task;
            return roadBlocks;
        }*/

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