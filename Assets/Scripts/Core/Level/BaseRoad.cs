using UniRx;
using UnityEditor;
using UnityEngine;

namespace Game.Core.Level
{
    public class BaseRoad : MonoBehaviour
    {
        protected readonly CompositeDisposable _subscriptions = new CompositeDisposable();

        [SerializeField] private Vector3 _startPointPosition;
        [SerializeField] private Vector3 _endPointPosition;
        [SerializeField] private Vector3 _endPointAdditionalRotation;
        [SerializeField] private Vector3 _startPointTangent;
        [SerializeField] private Vector3 _endPointTangent;

        public Vector3 GetStartPointLocalPosition()
        {
            return _startPointPosition;
        }

        public Vector3 GetStartPointWorldPosition()
        {
            return transform.position + transform.rotation * _startPointPosition;
        }

        public Vector3 GetEndPointWorldSpacePosition()
        {
            return transform.position + transform.rotation * _endPointPosition;
        }
        
        public Vector3 GetStartPointTangent()
        {
            return transform.position + transform.rotation * _startPointTangent;
        }

        public Vector3 GetEndPointTangent()
        {
            return transform.position + transform.rotation * _endPointTangent;
        }

        public Quaternion GetEndPointAdditionalRotation()
        {
            return Quaternion.Euler(_endPointAdditionalRotation);
        }

        protected virtual void OnDrawGizmos()
        {
            var startPoint = GetStartPointWorldPosition();
            var endPoint = GetEndPointWorldSpacePosition();

            var handle1 = GetStartPointTangent();
            var handle2 = GetEndPointTangent();

            Handles.DrawBezier(startPoint, endPoint, handle1, handle2, Color.green, null, 2f);

            Gizmos.DrawSphere(startPoint, 0.07f);
            Gizmos.DrawSphere(endPoint, 0.07f);

            Gizmos.color = Color.white;
            Gizmos.DrawCube(startPoint + transform.rotation * (_startPointTangent / 2), Vector3.one * 0.1f);
            Gizmos.DrawCube(startPoint - transform.rotation * (_startPointTangent / 2), Vector3.one * 0.1f);
            Gizmos.DrawCube(endPoint + transform.rotation * (_endPointTangent / 2), Vector3.one * 0.1f);
            Gizmos.DrawCube(endPoint - transform.rotation * (_endPointTangent / 2), Vector3.one * 0.1f);
            Gizmos.DrawLine(startPoint + transform.rotation * (_startPointTangent / 2), startPoint - transform.rotation * (_startPointTangent / 2));
            Gizmos.DrawLine(endPoint + transform.rotation * (_endPointTangent / 2), endPoint - transform.rotation * (_endPointTangent / 2));
        }

        protected virtual void OnDestroy()
        {
            _subscriptions.Dispose();
        }
    }
}