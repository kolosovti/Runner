using Game.Core.Model;

namespace Game
{
    public interface ICoreModel
    {
        ILevelLoadingModel LevelLoadingModel { get; }
        IPlayerModel PlayerModel { get; }
        ILevelModel LevelModel { get; }
        IInputModel InputModel { get; }
    }

    public class CoreModel : ICoreModel
    {
        public readonly LevelLoadingModel LevelLoading;
        public readonly PlayerModel Player;
        public readonly LevelModel Level;
        public readonly InputModel Input;

        ILevelLoadingModel ICoreModel.LevelLoadingModel => LevelLoading;
        IPlayerModel ICoreModel.PlayerModel => Player;
        ILevelModel ICoreModel.LevelModel => Level;
        IInputModel ICoreModel.InputModel => Input;


        public CoreModel(LevelLoadingModel levelLoadingModel, PlayerModel playerModel,
            LevelModel levelModel, InputModel inputModel)
        {
            LevelLoading = levelLoadingModel;
            Player = playerModel;
            Level = levelModel;
            Input = inputModel;
        }
    }
}