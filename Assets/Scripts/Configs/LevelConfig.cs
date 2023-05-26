using System.Collections.Generic;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private List<RoadBlockType> _roadSpawnOrder;

        public string SceneName => _sceneName;
        public List<RoadBlockType> RoadSpawnOrder => _roadSpawnOrder;
    }
}