using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Game.Core.Movement
{
    public class MockMovementStrategy : IMoveStrategy, IJumpStrategy
    {
        public Subject<Unit> PathComplete = new Subject<Unit>();
        IObservable<Unit> IMoveStrategy.PathComplete => PathComplete;

        public virtual void FixedTick()
        {
        }

        public virtual void Jump()
        {
        }
    }
}
