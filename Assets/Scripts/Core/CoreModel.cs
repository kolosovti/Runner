using Game.Core.Model;

namespace Game
{
    public interface ICoreModel
    {
        ILevelLoadingModel LevelLoadingModel { get; }
        ILevelModel LevelModel { get; }
        IPlayerModel PlayerModel { get; }
    }

    public class CoreModel : ICoreModel
    {
        public readonly LevelLoadingModel LevelLoading;
        public readonly LevelModel Level;
        public readonly PlayerModel Player;

        ILevelLoadingModel ICoreModel.LevelLoadingModel => LevelLoading;
        ILevelModel ICoreModel.LevelModel => Level;
        IPlayerModel ICoreModel.PlayerModel => Player;


        public CoreModel(LevelLoadingModel levelLoadingModel, LevelModel levelModel, PlayerModel playerModel)
        {
            LevelLoading = levelLoadingModel;
            Level = levelModel;
            Player = playerModel;
        }
    }
}