using System;
using Game.Core.Stats;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Game.Core.Level
{
    public class RoadWithObstacle : BaseRoad, IObstacleProvider, IStatisticsProvider
    {
        [SerializeField] private Collider _obstacleCollider;
        [SerializeField] private Collider _obstacleCompleteCollider;
        [SerializeField] private StatsType _statsType;
        [SerializeField] private ObstacleType _obstacleType;

        private Subject<StatisticsContainer> _obstacleComplete = new Subject<StatisticsContainer>();
        private Subject<ObstacleType> _obstacleEnter = new Subject<ObstacleType>();
        private Subject<ObstacleType> _obstacleExit = new Subject<ObstacleType>();

        public IObservable<StatisticsContainer> Complete => _obstacleComplete;
        public IObservable<ObstacleType> ObstacleEnter => _obstacleEnter;
        public IObservable<ObstacleType> ObstacleExit => _obstacleExit;

        private void Start()
        {
            _obstacleCompleteCollider.OnTriggerEnterAsObservable().First().Subscribe(ObstacleCompleteColliderEnter)
                .AddTo(_subscriptions);
            _obstacleCollider.OnTriggerEnterAsObservable().First().Subscribe(ObstacleColliderEnter)
                .AddTo(_subscriptions);
            _obstacleCollider.OnTriggerExitAsObservable().First().Subscribe(ObstacleColliderExit)
                .AddTo(_subscriptions);
        }

        protected virtual void ObstacleCompleteColliderEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _obstacleComplete.OnNext(new StatisticsContainer(_statsType, 1));
            }
        }

        protected virtual void ObstacleColliderEnter(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _obstacleEnter.OnNext(_obstacleType);
            }
        }

        protected virtual void ObstacleColliderExit(Collider other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _obstacleExit.OnNext(_obstacleType);
            }
        }
    }
}