using Game.Core.Model;
using Game.Helpers;
using UnityEngine;

namespace Game.Core
{
    public class GameEntryPoint : MonoBehaviour
    {
        private CoreContext _coreContext;

        private void Start()
        {
            var levelLoadingModel = new LevelLoadingModel(Services.Configs.LevelConfig);
            var levelModel = new LevelModel(Services.Configs.LevelConfig);
            var playerModel = new PlayerModel();
            _coreContext = new CoreContext(levelLoadingModel, levelModel, playerModel);
            _coreContext.Init();
            _coreContext.Start();
        }
    }
}