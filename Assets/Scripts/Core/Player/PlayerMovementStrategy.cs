using System;
using Game.Configs;
using Game.Core.Model;
using Game.Helpers;
using UniRx;
using UnityEngine;

namespace Game.Core.Movement
{
    public class PlayerMovementStrategy : MockMovementStrategy
    {
        private readonly IPlayerModel _playerModel;
        private readonly Rigidbody _rigidbody;
        private readonly BezierSegmentSettings _path;
        private readonly MovementConfig _movementConfig;

        private float _elapsedTime = 0f;
        private float _pathLenght;
        private Vector3 _startPoint;
        private Vector3 _previousPosition;
        private float _yForce;

        public float YForce => _yForce;

        public PlayerMovementStrategy(IPlayerModel playerModel, Rigidbody rigidbody, MovementConfig movementConfig,
            BezierSegmentSettings path, float savedYForce = 0f)
        {
            _playerModel = playerModel;
            _rigidbody = rigidbody;
            _path = path;
            _movementConfig = movementConfig;
            _yForce = savedYForce;

            _pathLenght = Bezier.GetLength(_path);
            _startPoint = Bezier.GetPoint(_path, 0f);
            _previousPosition = _startPoint;
        }

        public override void Jump()
        {
            if (_yForce > 0)
            {
                _yForce += _movementConfig.JumpForce;
            }
            else
            {
                _yForce = _movementConfig.JumpForce;
            }
        }

        //TODO: add interpolation for smooth movement
        public override void FixedTick()
        {
            float time = _pathLenght / _movementConfig.RunnerSpeed;

            if (_elapsedTime < time)
            {
                float t = _elapsedTime / time;
                Vector3 position = Bezier.GetPoint(_path, t);

                if (_yForce <= 0)
                {
                    var absYForce = Math.Abs(_yForce);
                    float distanceToGround = _yForce;
                    if (Physics.Raycast(_rigidbody.position, Vector3.down, out var hit, absYForce))
                    {
                        distanceToGround = hit.distance;
                    }

                    if (distanceToGround > -absYForce)
                    {
                        _yForce = -distanceToGround;
                    }
                }

                var yPos = _rigidbody.position.y + _yForce;
                _rigidbody.position = new Vector3(position.x, yPos, position.z);

                if (position != _previousPosition)
                {
                    Vector3 nextPosition = Bezier.GetPoint(_path, t + Time.fixedDeltaTime);
                    Vector3 direction = (nextPosition - position).normalized;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    _rigidbody.rotation = Quaternion.Euler(Vector3.up * rotation.eulerAngles.y);
                }

                _elapsedTime += Time.fixedDeltaTime;

                if (_playerModel.IsGrounded.Value)
                {
                    _yForce = 0f;
                }
                else
                {
                    _yForce -= _movementConfig.GravityForce * Time.fixedDeltaTime;
                }
            }
            else
            {
                PathComplete.OnNext(Unit.Default);
            }
        }
    }
}
