using System;
using UniRx;

namespace Game
{
    public interface IInputModel
    {
        IObservable<Unit> Jump { get; }
    }

    public class InputModel : IInputModel
    {
        public Subject<Unit> Jump = new Subject<Unit>();

        IObservable<Unit> IInputModel.Jump => Jump.AsUnitObservable();

        public void InvokeJump()
        {
            Jump.OnNext(Unit.Default);
        }
    }
}