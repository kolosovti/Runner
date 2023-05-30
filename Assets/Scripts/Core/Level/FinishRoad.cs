using System;
using Game.Core.Level;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game
{
    public class FinishRoad : BaseRoad, IFinishProvider
    {
        [SerializeField] private Collider _finishCollider;

        private Subject<Unit> _finish = new Subject<Unit>();
        public IObservable<Unit> Finish => _finish;

        private void Start()
        {
            _finishCollider.OnTriggerEnterAsObservable().Subscribe(FinishColliderEnter)
                .AddTo(_subscriptions);
        }

        protected virtual void FinishColliderEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _finish.OnNext(Unit.Default);
                _subscriptions.Dispose();
            }
        }
    }
}