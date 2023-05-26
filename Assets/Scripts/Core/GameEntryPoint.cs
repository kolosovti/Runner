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
            var levelModel = new LevelModel(Services.Configs.LevelConfig);
            _coreContext = new CoreContext(levelModel);
            _coreContext.Init();
            _coreContext.Start();
        }
    }
}