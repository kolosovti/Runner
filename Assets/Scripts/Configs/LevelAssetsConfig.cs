using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Configs
{
    public enum RoadBlockType
    {
        Simple,
        Hole,
        LongHole,
        Fence,
        Saw,
        TurnRight,
        TurnLeft,
        Finish
    }

    [CreateAssetMenu(fileName = "LevelAssetsConfig", menuName = "Configs/LevelAssetsConfig")]
    public class LevelAssetsConfig : ScriptableObject
    {
        [SerializeField] private List<RoadPrefab> _roadPrefabs;
        [SerializeField] private AssetReference _playerPrefabReference;
        [SerializeField] private AssetReference _winWindowReference;
        [SerializeField] private AssetReference _deathWindowReference;
        [SerializeField] private AssetReference _healthViewReference;

        public List<RoadPrefab> RoadPrefabs => _roadPrefabs;
        public AssetReference PlayerPrefabReference => _playerPrefabReference;
        public AssetReference WinWindowReference => _winWindowReference;
        public AssetReference DeathWindowReference => _deathWindowReference;
        public AssetReference HealthViewReference => _healthViewReference;
    }

    [Serializable]
    public class RoadPrefab
    {
        public RoadBlockType Type;
        public AssetReference PrefabReference;
    }
}