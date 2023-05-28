using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Configs;
using Game.Helpers;
using Game.System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Game.Core.Controllers
{
    public class AssetsController : BaseContextController
    {
        private Dictionary<RoadBlockType, GameObject> _roadPrefabs =
            new Dictionary<RoadBlockType, GameObject>();

        private GameObject _playerPrefab;

        private List<AsyncOperationHandle> _loadedResources = new List<AsyncOperationHandle>();

        public AssetsController(ContextManager contextManager) : base(contextManager)
        {
        }

        public GameObject GetPlayerPrefab()
        {
            return _playerPrefab;
        }

        public bool TryGetRoadPrefabByType(RoadBlockType type, out GameObject prefab)
        {
            if (_roadPrefabs.ContainsKey(type))
            {
                prefab = _roadPrefabs[type];
                return true;
            }

            Debug.LogError($"Road prefab with type {type} not exist in preloaded assets");
            prefab = null;
            return false;
        }

        public async Task LoadPlayerPrefab()
        {
            var player = await LoadPrefabFromAddressables(Services.Configs.LevelAssetsConfig.PlayerPrefabReference);
            _playerPrefab = player;
        }

        public async Task LoadRoadPrefabs()
        {
            foreach (var roadPrefab in Services.Configs.LevelAssetsConfig.RoadPrefabs)
            {
                if (_roadPrefabs.ContainsKey(roadPrefab.Type))
                {
                    Debug.LogError($"Double entry road type {roadPrefab.Type} in prefabs List");
                }
                else
                {
                    var loadedPrefab = await LoadPrefabFromAddressables(roadPrefab.PrefabReference);
                    _roadPrefabs.Add(roadPrefab.Type, loadedPrefab);
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