using Game.Core.Model;
using Game.Helpers;
using UnityEngine;

namespace Game.Core
{
    public class GameEntryPoint : Singleton<GameEntryPoint>
    {
        private CoreContext _coreContext;

        private void Start()
        {
            var levelLoadingModel = new LevelLoadingModel(Services.Configs.LevelConfig);
            var levelModel = new LevelModel(Services.Configs.LevelConfig);
            var playerModel = new PlayerModel();
            var inputModel = new InputModel();
            _coreContext = new CoreContext(levelLoadingModel, levelModel, playerModel, inputModel);
            _coreContext.Init();
            _coreContext.Start();
        }

        private void Update()
        {
            _coreContext.Tick();
        }
    }
}