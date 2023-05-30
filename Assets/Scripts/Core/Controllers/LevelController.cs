using System.Collections.Generic;
using Game.Core.Model;
using Game.Core.Level;
using Game.Helpers;
using Game.System;
using UniRx;
using UnityEngine;

namespace Game.Core.Controllers
{
    public class LevelController : BaseContextController
    {
        private readonly ILevelLoadingModel _levelLoadingModel;
        private readonly LevelModel _levelModel;

        public LevelController(ILevelLoadingModel levelLoadingModel, LevelModel levelModel, ContextManager contextManager) 
            : base(contextManager)
        {
            _levelLoadingModel = levelLoadingModel;
            _levelModel = levelModel;

            _levelLoadingModel.LevelLoaded.First().Subscribe(x => OnSceneLoaded()).AddTo(_subscriptions);
        }

        //TODO: можно вынести предзагрузку ассетов в конструктор, подписываться на событие окончания предзагрузки в AssetsModel и по нему спавнить объекты
        private async void OnSceneLoaded()
        {
            var levelActivationLocker = new ReactiveProperty<bool>();
            _levelLoadingModel.RegisterSceneActivationLocker(levelActivationLocker);

            var assetsController = GetController<AssetsController>();
            await assetsController.LoadRoadPrefabs();
            SpawnLevel();

            levelActivationLocker.Value = true;
            _levelModel.LevelSpawned();
        }

        private void SpawnLevel()
        {
            var root = new GameObject();
            root.name = "Road root";
            root.transform.position = Vector3.zero;

            var assetsController = GetController<AssetsController>();
            BaseRoad previousBlock = null;
            foreach (var roadBlockType in _levelModel.Config.RoadSpawnOrder)
            {
                if (assetsController.TryGetRoadPrefabByType(roadBlockType, out var prefab))
                {
                    var roadBlock = Object.Instantiate(prefab, root.transform).GetComponent<BaseRoad>();
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
                    _levelModel.AddRoadBlockInSequence(roadBlock);
                }
            }
        }
    }
}