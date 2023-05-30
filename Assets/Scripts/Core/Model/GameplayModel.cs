using System;
using UniRx;

namespace Game.Core.Model
{
    public interface IGameplayModel
    {
        IObservable<Unit> Revive { get; }
        IObservable<Unit> Finish { get; }
        IObservable<Unit> Death { get; }
    }

    public class GameplayModel : IGameplayModel
    {
        public Subject<Unit> Revive = new Subject<Unit>();
        public Subject<Unit> Finish = new Subject<Unit>();
        public Subject<Unit> Death = new Subject<Unit>();

        IObservable<Unit> IGameplayModel.Revive => Revive;
        IObservable<Unit> IGameplayModel.Finish => Finish;
        IObservable<Unit> IGameplayModel.Death => Death;
    }
}