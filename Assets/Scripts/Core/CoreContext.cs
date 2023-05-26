using Game.Core.Controllers;
using Game.Core.Model;
using Game.System;

namespace Game.Core
{
    public class CoreContext : ContextManager
    {
        private LevelModel _levelModel;

        public CoreContext(LevelModel levelModel)
        {
            _levelModel = levelModel;
        }

        protected override void CreateControllers()
        {
            CreateController(new AssetsController(this));
            CreateController(new LevelController(_levelModel, this));
        }
    }
}