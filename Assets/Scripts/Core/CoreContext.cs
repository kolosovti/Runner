using Game.Core.Controllers;
using Game.Core.Model;
using Game.System;

namespace Game.Core
{
    public class CoreContext : ContextManager
    {
        private CoreModel _coreModel;

        public CoreContext(LevelLoadingModel levelLoadingModel, LevelModel levelModel, 
            PlayerModel playerModel, InputModel inputModel)
        {
            _coreModel = new CoreModel(
                levelLoadingModel,
                playerModel,
                levelModel,
                inputModel);
        }

        protected override void CreateControllers()
        {
            CreateController(new LevelLoadingController(_coreModel.LevelLoading, this));
            CreateController(new AssetsController(this));
            CreateController(new LevelController(_coreModel.LevelLoading, _coreModel.Level, this));
            CreateController(new PlayerController(_coreModel.LevelLoading, _coreModel.Input, _coreModel.Player, this));
        }
    }
}