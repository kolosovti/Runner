using System;
using System.Collections.Generic;
using Game.Core.Level;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class ObstacleContainer
    {
        [SerializeField] private ObstacleType _type;
        [SerializeField] private float _damage;

        public ObstacleType Type => _type;
        public float Damage => _damage;
    }

    [CreateAssetMenu(fileName = "ObstaclesConfig", menuName = "Configs/ObstaclesConfig")]
    public class ObstaclesConfig : ScriptableObject
    {
        [SerializeField] private List<ObstacleContainer> _obstacles;
        
        public IReadOnlyList<ObstacleContainer> Obstacles => _obstacles;

        public bool TryGetObstacleConfigByType(ObstacleType type, out ObstacleContainer config)
        {
            foreach (var obstacleConfig in _obstacles)
            {
                if (obstacleConfig.Type == type)
                {
                    config = obstacleConfig;
                    return true;
                }
            }

            config = null;
            return false;
        }
    }
}