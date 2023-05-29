using System;
using Game.Configs;
using Game.Helpers;
using UniRx;
using UnityEngine;

namespace Game.Core.Movement
{
    public class BaseMoveStrategy : IMoveStrategy
    {
        private readonly Rigidbody _rigidbody;
        private readonly BezierSegmentSettings _path;
        private readonly MovementConfig _movementConfig;

        private float _elapsedTime = 0f;
        private float _pathLenght;
        private Vector3 _startPoint;
        private Vector3 _previousPosition;

        public Subject<Unit> PathComplete = new Subject<Unit>();
        IObservable<Unit> IMoveStrategy.PathComplete => PathComplete;

        public BaseMoveStrategy(Rigidbody rigidbody, MovementConfig movementConfig, BezierSegmentSettings path)
        {
            _rigidbody = rigidbody;
            _path = path;
            _movementConfig = movementConfig;

            _pathLenght = Bezier.GetLength(_path);
            _startPoint = Bezier.GetPoint(_path, 0f);
            _previousPosition = _startPoint;
        }

        //TODO: add interpolation for smooth movement
        public virtual void FixedTick()
        {
            float time = _pathLenght / _movementConfig.RunnerSpeed;

            if (_elapsedTime < time)
            {
                float t = _elapsedTime / time;
                Vector3 position = Bezier.GetPoint(_path, t);
                _rigidbody.position = position;

                if (position != _previousPosition)
                {
                    Vector3 nextPosition = Bezier.GetPoint(_path, t + Time.fixedDeltaTime);
                    Vector3 direction = (nextPosition - position).normalized;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    _rigidbody.rotation = rotation;
                }

                _elapsedTime += Time.fixedDeltaTime;
            }
            else
            {
                PathComplete.OnNext(Unit.Default);
            }
        }
    }
}
