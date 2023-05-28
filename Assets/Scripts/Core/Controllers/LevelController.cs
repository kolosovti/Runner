using System.Collections.Generic;
using Game.Core.Model;
using Game.Core.Road;
using Game.Helpers;
using Game.System;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Game.Core.Controllers
{
    public class LevelController : BaseContextController
    {
        private readonly ILevelLoadingModel _levelLoadingModel;
        private readonly ILevelModel _levelModel;
        
        private List<AsyncOperationHandle> _loadedResources = new List<AsyncOperationHandle>();

        public LevelController(ILevelLoadingModel levelLoadingModel, ILevelModel levelModel, ContextManager contextManager) 
            : base(contextManager)
        {
            _levelLoadingModel = levelLoadingModel;
            _levelModel = levelModel;

            _levelLoadingModel.LevelLoaded.First().Subscribe(x => OnSceneLoaded()).AddTo(_subscriptions);
        }

        //TODO: можно вынести предзагрузку ассетов в конструктор, подписываться на событие в AssetsModel и по нему спавнить объекты
        private async void OnSceneLoaded()
        {
            var levelActivationLocker = new ReactiveProperty<bool>();
            _levelLoadingModel.RegisterSceneActivationLocker(levelActivationLocker);

            var assetsController = GetController<AssetsController>();
            await assetsController.LoadRoadPrefabs();
            SpawnLevel();

            levelActivationLocker.Value = true;
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
                if (assetsController.TryGetRoadPrefabByType(roadBlockType, out var prefab))
                {
                    var roadBlock = Object.Instantiate(prefab, root.transform).GetComponent<BaseRoadObject>();
                    if (previousBlock != null)
                    {
                        roadBlock.transform.rotation = previousBlock.transform.rotation * previousBlock.GetEndPointAdditionalRotation();

                        roadBlock.transform.position = previousBlock.GetEndPointWorldSpacePosition() - 
                                                       roadBlock.transform.rotation * roadBlock.GetStartPointLocalPosition();
                    }
                    else
                    {
                        roadBlock.transform.position = _levelModel.Config.RoadInitialPosition;
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