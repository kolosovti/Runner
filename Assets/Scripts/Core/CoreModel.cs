using Game.Core.Model;

namespace Game
{
    public interface ICoreModel
    {
        ILevelLoadingModel LevelLoadingModel { get; }
        IStatisticsModel StatisticsModel { get; }
        IPlayerModel PlayerModel { get; }
        IGameplayModel GameplayModel { get; }
        ILevelModel LevelModel { get; }
        IInputModel InputModel { get; }
    }

    public class CoreModel : ICoreModel
    {
        public readonly LevelLoadingModel LevelLoading;
        public readonly StatisticsModel Statistics;
        public readonly PlayerModel Player;
        public readonly GameplayModel Gameplay;
        public readonly LevelModel Level;
        public readonly InputModel Input;

        ILevelLoadingModel ICoreModel.LevelLoadingModel => LevelLoading;
        IStatisticsModel ICoreModel.StatisticsModel => Statistics;
        IPlayerModel ICoreModel.PlayerModel => Player;
        IGameplayModel ICoreModel.GameplayModel => Gameplay;
        ILevelModel ICoreModel.LevelModel => Level;
        IInputModel ICoreModel.InputModel => Input;

        public CoreModel(
            LevelLoadingModel levelLoadingModel,
            StatisticsModel statsModel,
            PlayerModel playerModel,
            GameplayModel gameplayModel,
            LevelModel levelModel, 
            InputModel inputModel)
        {
            LevelLoading = levelLoadingModel;
            Statistics = statsModel;
            Player = playerModel;
            Gameplay = gameplayModel;
            Level = levelModel;
            Input = inputModel;
        }
    }
}