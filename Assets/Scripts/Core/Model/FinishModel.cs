using UniRx;

namespace Game.Core.Model
{
    public interface IFinishModel
    {
        IReadOnlyReactiveProperty<bool> Finish { get; }
    }

    public class FinishModel : IFinishModel
    {
        public ReactiveProperty<bool> Finish = new ReactiveProperty<bool>(false);

        IReadOnlyReactiveProperty<bool> IFinishModel.Finish => Finish;
    }
}