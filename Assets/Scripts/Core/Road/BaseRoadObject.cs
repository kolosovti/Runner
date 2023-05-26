using System.Numerics;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Game.Core.Road
{
    public class BaseRoadObject : MonoBehaviour, IRoadObject
    {
        [SerializeField] private Vector3 _startPointPosition;
        [SerializeField] private Vector3 _endPointPosition;
        [SerializeField] private Vector3 _endPointAdditionalRotation;

        public Vector3 GetStartPointLocalPosition()
        {
            return _startPointPosition;
        }

        public Vector3 GetEndPointWorldSpacePosition()
        {
            return transform.position + transform.rotation * _endPointPosition;
        }

        public Quaternion GetEndPointAdditionalRotation()
        {
            return Quaternion.Euler(_endPointAdditionalRotation);
        }

        protected virtual void OnDrawGizmos()
        {
            //Draw end point
            Gizmos.color = Color.blue;
            var startPoint = transform.position + transform.rotation * (_startPointPosition);
            Gizmos.DrawSphere(startPoint, 0.04f);

            //Draw end point and road block exit direction
            Gizmos.color = Color.red;
            var endPointFrom = transform.position + transform.rotation * ( _endPointPosition);
            var endPointTo = transform.rotation * Quaternion.Euler(_endPointAdditionalRotation) * Vector3.forward;
            Gizmos.DrawSphere(endPointFrom, 0.06f);
            Gizmos.DrawLine(endPointFrom, endPointFrom + endPointTo);
        }
    }
}