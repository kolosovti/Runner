using System;
using UniRx;

namespace Game.Core.Movement
{
    public interface IMoveStrategy
    {
        IObservable<Unit> PathComplete { get; }
        void FixedTick();
    }
}