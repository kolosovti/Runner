using System.Collections.Generic;
using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private List<RoadBlockType> _roadSpawnOrder;
        [SerializeField] private Vector3 _roadInitialPosition;
        [SerializeField] private Vector3 _playerInitialPosition;

        public string SceneName => _sceneName;
        public List<RoadBlockType> RoadSpawnOrder => _roadSpawnOrder;
        public Vector3 RoadInitialPosition => _roadInitialPosition;
        public Vector3 PlayerInitialPosition => _playerInitialPosition;
    }
}