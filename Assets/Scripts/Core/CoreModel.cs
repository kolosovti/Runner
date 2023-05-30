using Game.Core.Model;

namespace Game
{
    public interface ICoreModel
    {
        ILevelLoadingModel LevelLoadingModel { get; }
        IStatisticsModel StatisticsModel { get; }
        IPlayerModel PlayerModel { get; }
        IFinishModel FinishModel { get; }
        ILevelModel LevelModel { get; }
        IInputModel InputModel { get; }
    }

    public class CoreModel : ICoreModel
    {
        public readonly LevelLoadingModel LevelLoading;
        public readonly StatisticsModel Statistics;
        public readonly PlayerModel Player;
        public readonly FinishModel Finish;
        public readonly LevelModel Level;
        public readonly InputModel Input;

        ILevelLoadingModel ICoreModel.LevelLoadingModel => LevelLoading;
        IStatisticsModel ICoreModel.StatisticsModel => Statistics;
        IPlayerModel ICoreModel.PlayerModel => Player;
        IFinishModel ICoreModel.FinishModel => Finish;
        ILevelModel ICoreModel.LevelModel => Level;
        IInputModel ICoreModel.InputModel => Input;

        public CoreModel(
            LevelLoadingModel levelLoadingModel,
            StatisticsModel statsModel,
            PlayerModel playerModel,
            FinishModel finishModel,
            LevelModel levelModel, 
            InputModel inputModel)
        {
            LevelLoading = levelLoadingModel;
            Statistics = statsModel;
            Player = playerModel;
            Finish = finishModel;
            Level = levelModel;
            Input = inputModel;
        }
    }
}