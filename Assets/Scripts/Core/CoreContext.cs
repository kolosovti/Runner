using Game.Core.Controllers;
using Game.Core.Model;
using Game.System;

namespace Game.Core
{
    public class CoreContext : ContextManager
    {
        private CoreModel _coreModel;

        public CoreContext(LevelLoadingModel levelLoadingModel, LevelModel levelModel, PlayerModel playerModel)
        {
            _coreModel = new CoreModel(
                levelLoadingModel,
                levelModel,
                playerModel);
        }

        protected override void CreateControllers()
        {
            CreateController(new LevelLoadingController(_coreModel.LevelLoading, this));
            CreateController(new AssetsController(this));
            CreateController(new LevelController(_coreModel.LevelLoading, _coreModel.Level, this));
            CreateController(new PlayerController(_coreModel.LevelLoading, _coreModel.Player, this));
        }
    }
}