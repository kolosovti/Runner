using UnityEngine;

namespace Game.Core.Road
{
    public interface IRoadObject
    {
        Vector3 GetStartPointLocalPosition();
        Vector3 GetEndPointWorldSpacePosition();

        Quaternion GetEndPointAdditionalRotation();
        //IMoveStrategy GetMoveStrategy();
    }
}