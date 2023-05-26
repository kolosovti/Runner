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
            Gizmos.color = Color.red;
            var startPoint = transform.position + transform.rotation * ( _endPointPosition);
            Gizmos.DrawSphere(startPoint, 0.04f);

            var endPoint = transform.rotation * Quaternion.Euler(_endPointAdditionalRotation) * Vector3.forward;
            Gizmos.DrawLine(startPoint, startPoint + endPoint);
        }
    }
}