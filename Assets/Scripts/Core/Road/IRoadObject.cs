using UnityEngine;

namespace Game.Core.Road
{
    public interface IRoadObject
    {
        Vector3 GetEndPointPosition();

        Quaternion GetEndPointAdditionalRotation();
        //IMoveStrategy GetMoveStrategy();
    }
}