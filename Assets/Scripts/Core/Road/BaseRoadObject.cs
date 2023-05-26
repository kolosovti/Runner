using UnityEngine;

namespace Game.Core.Road
{
    public class BaseRoadObject : MonoBehaviour, IRoadObject
    {
        [SerializeField] private Vector3 _endPointPosition;
        [SerializeField] private Vector3 _endPointAdditionalRotation;

        public Vector3 GetEndPointPosition()
        {
            return transform.position + _endPointPosition;
        }

        public Quaternion GetEndPointAdditionalRotation()
        {
            return transform.rotation * Quaternion.Euler(_endPointAdditionalRotation);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            var endPoint = _endPointAdditionalRotation.magnitude > 0
                ? _endPointAdditionalRotation
                : Vector3.forward;
            Gizmos.DrawLine(transform.position + _endPointPosition, transform.position + _endPointPosition + endPoint);
        }
    }
}