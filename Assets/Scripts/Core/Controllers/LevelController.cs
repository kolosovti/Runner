using System;
using System.Collections.Generic;
using System.Linq;
using Game.Configs;
using Game.Core.Model;
using Game.Core.Level;
using Game.Helpers;
using Game.System;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

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

            // Раскомментить если нужна дорога из конфига
            //foreach (var roadBlockType in _levelModel.Config.RoadSpawnOrder)
            foreach (var roadBlockType in GetRandomRoad())
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

        private RoadBlockType[] GetRandomRoad(int lenght = 10)
        {
            var road = new RoadBlockType[lenght];
            road[0] = RoadBlockType.Simple;
            road[lenght - 1] = RoadBlockType.Finish;
            for (var i = 1; i < road.Length - 2; i++)
            {
                road[i] = RollRandomRoadBlockWithoutFinish();
            }
            return road;
        }

        //TODO: написать нормальный рандомайзер
        private RoadBlockType RollRandomRoadBlockWithoutFinish()
        {
            var road = RoadBlockType.Finish;
            while (road == RoadBlockType.Finish)
            {
                var values = Enum.GetValues(typeof(RoadBlockType)).Cast<RoadBlockType>().ToList();
                var item = UnityEngine.Random.Range(1, values.Count - 1);
                road = (RoadBlockType)values[item];
            }

            return road;
        }
    }
}