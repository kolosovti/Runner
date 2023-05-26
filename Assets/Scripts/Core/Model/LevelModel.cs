using Game.Configs;

namespace Game.Core.Model
{
    public interface ILevelModel
    {
        LevelConfig Config { get; }
    }

    public class LevelModel : ILevelModel
    {
        private LevelConfig _config;

        public LevelModel(LevelConfig levelConfig)
        {
            _config = levelConfig;
        }

        LevelConfig ILevelModel.Config => _config;
    }
}