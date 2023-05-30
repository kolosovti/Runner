using Game.Core.Model;
using Game.Helpers;

namespace Game.Core
{
    public class GameEntryPoint : Singleton<GameEntryPoint>
    {
        private CoreContext _coreContext;

        private void Start()
        {
            LoadCoreContext();
        }

        public void LoadCoreContext()
        {
            var levelLoadingModel = new LevelLoadingModel(Services.Configs.LevelConfig);
            var levelModel = new LevelModel(Services.Configs.LevelConfig);
            var playerModel = new PlayerModel();
            var inputModel = new InputModel();
            var statsModel = new StatisticsModel();
            var finishModel = new GameplayModel();
            _coreContext = new CoreContext(levelLoadingModel, statsModel, playerModel, finishModel, levelModel, inputModel);
            _coreContext.Init();
            _coreContext.Start();
        }

        private void Update()
        {
            _coreContext.Tick();
        }

        private void FixedUpdate()
        {
            _coreContext.FixedTick();
        }
    }
}