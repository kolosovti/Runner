using System.Collections;
using System.Collections.Generic;
using Game.Configs;
using Game.Core.Movement;
using UnityEngine;

namespace Game.Core.Movement
{
    public class BaseMoveStrategy : IMoveStrategy
    {
        private Rigidbody _rigidbody;
        private MovementConfig _config;

        public BaseMoveStrategy(Rigidbody rigidbody, MovementConfig config)
        {
            _rigidbody = rigidbody;
            _config = config;
        }

        public virtual void FixedTick()
        {
            UpdateVelocity();
            UpdateRotation();
        }

        protected virtual void UpdateVelocity()
        {
            _rigidbody.velocity = _rigidbody.transform.rotation * new Vector3(0f, _rigidbody.velocity.y, _config.RunnerSpeed);
        }

        protected virtual void UpdateRotation()
        {
            //_rigidbody.MoveRotation();
        }
    }
}
