using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "Configs", menuName = "Configs/Configs")]
    public class Configs : ScriptableObject
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private LevelAssetsConfig _levelAssetsConfig;
        [SerializeField] private PlayerConfig _playerConfig;
        [SerializeField] private MovementConfig _playerMovementConfig;
        [SerializeField] private CameraConfig _playerCameraConfig;

        public LevelConfig LevelConfig => _levelConfig;
        public LevelAssetsConfig LevelAssetsConfig => _levelAssetsConfig;
        public PlayerConfig PlayerConfig => _playerConfig;
        public MovementConfig PlayerMovementConfig => _playerMovementConfig;
        public CameraConfig PlayerCameraConfig => _playerCameraConfig;
    }
}