using Game.Core.Level;
using Game.Core.Model;
using Game.System;
using UniRx;

namespace Game
{
    public class FinishController : BaseContextController
    {
        private readonly ILevelModel _levelModel;
        private readonly FinishModel _finishModel;

        public FinishController(ILevelModel levelModel, FinishModel finishModel,
            ContextManager contextManager) : base(contextManager)
        {
            _levelModel = levelModel;
            _finishModel = finishModel;
        }

        public override void ConnectController()
        {
            base.ConnectController();

            _levelModel.LevelSpawned.First().Subscribe(x => SubscribeToLevelStats()).AddTo(_subscriptions);
        }

        private void SubscribeToLevelStats()
        {
            foreach (var baseRoadObject in _levelModel.RoadObjectSequence)
            {
                (baseRoadObject as IFinishProvider)?.Finish.First().Subscribe(x => HandleFinish())
                    .AddTo(_subscriptions);
            }
        }

        private void HandleFinish()
        {
            _finishModel.Finish.Value = true;
        }
    }
}