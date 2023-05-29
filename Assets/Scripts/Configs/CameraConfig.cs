using System.Collections.Generic;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "CameraConfig", menuName = "Configs/CameraConfig")]
    public class CameraConfig : ScriptableObject
    {
        [SerializeField] private Vector3 _playerCameraPosition;
        [SerializeField] private Vector3 _playerCameraRotation;
        
        public Vector3 PlayerCameraPosition => _playerCameraPosition;
        public Vector3 PlayerCameraRotation => _playerCameraRotation;
    }
}