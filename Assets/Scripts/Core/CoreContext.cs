using Game.Core.Controllers;
using Game.Core.Model;
using Game.System;

namespace Game.Core
{
    public class CoreContext : ContextManager
    {
        private CoreModel _coreModel;

        public CoreContext(
            LevelLoadingModel levelLoadingModel,
            StatisticsModel statisticsModel,
            PlayerModel playerModel,
            FinishModel finishModel,
            LevelModel levelModel,
            InputModel inputModel)
        {
            _coreModel = new CoreModel(
                levelLoadingModel,
                statisticsModel,
                playerModel,
                finishModel,
                levelModel,
                inputModel);
        }

        protected override void CreateControllers()
        {
            CreateController(new LevelLoadingController(_coreModel.LevelLoading, this));
            CreateController(new AssetsController(this));
            CreateController(new LevelController(_coreModel.LevelLoading, _coreModel.Level, this));
            CreateController(new InputController(_coreModel.Input, this));
            CreateController(new PlayerController(_coreModel.LevelLoading, _coreModel.Finish, _coreModel.Player,
                _coreModel.Level, _coreModel.Input,  this));
            CreateController(new StatisticsController(_coreModel.Level, _coreModel.Statistics, this));
            CreateController(new FinishController(_coreModel.Level, _coreModel.Finish, this));
        }
    }
}