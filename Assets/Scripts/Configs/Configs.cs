using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "Configs", menuName = "Configs/Configs")]
    public class Configs : ScriptableObject
    {
        [SerializeField] private LevelConfig _levelConfig;
        [SerializeField] private LevelAssetsConfig _levelAssetsConfig;

        public LevelConfig LevelConfig => _levelConfig;
        public LevelAssetsConfig LevelAssetsConfig => _levelAssetsConfig;
    }
}