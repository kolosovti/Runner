using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Configs;
using Game.Core.Road;
using Game.Helpers;
using Game.System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Core.Controllers
{
    public class AssetsController : BaseContextController
    {
        private Dictionary<RoadBlockType, BaseRoadObject> _preloadedRoadPrefabs =
            new Dictionary<RoadBlockType, BaseRoadObject>();

        private List<AsyncOperationHandle> _loadedResources = new List<AsyncOperationHandle>();

        public AssetsController(ContextManager contextManager) : base(contextManager)
        {
        }

        public bool TryGetPreloadedRoadPrefabByType(RoadBlockType type, out BaseRoadObject prefab)
        {
            if (_preloadedRoadPrefabs.ContainsKey(type))
            {
                prefab = _preloadedRoadPrefabs[type];
                return true;
            }

            Debug.LogError($"Road prefab with type {type} not exist in preloaded assets");
            prefab = null;
            return false;
        }

        public async Task LoadAssetsFromAddressables()
        {
            await PreloadRoadPrefabs();
        }

        private async Task PreloadRoadPrefabs()
        {
            foreach (var roadPrefab in Services.Configs.LevelAssetsConfig.RoadPrefabs)
            {
                if (_preloadedRoadPrefabs.ContainsKey(roadPrefab.Type))
                {
                    Debug.LogError($"Double entry road type {roadPrefab.Type} in prefabs List");
                }
                else
                {
                    var loadedPrefab = await LoadPrefabFromAddressables(roadPrefab.PrefabReference);
                    _preloadedRoadPrefabs.Add(roadPrefab.Type, loadedPrefab.GetComponent<BaseRoadObject>());
                }
            }
        }

        private async Task<GameObject> LoadPrefabFromAddressables(AssetReference assetReference)
        {
            var handle = assetReference.LoadAssetAsync<GameObject>();
            handle.AddTo(_loadedResources);
            await handle.Task;
            return handle.Result;
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