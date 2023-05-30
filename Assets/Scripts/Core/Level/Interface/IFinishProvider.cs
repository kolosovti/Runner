using System;
using UniRx;

namespace Game.Core.Level
{
    public interface IFinishProvider
    {
        IObservable<Unit> Finish { get; }
    }
}