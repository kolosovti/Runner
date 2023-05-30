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

        public List<RoadPrefab> RoadPrefabs => _roadPrefabs;
        public AssetReference PlayerPrefabReference => _playerPrefabReference;
    }

    [Serializable]
    public class RoadPrefab
    {
        public RoadBlockType Type;
        public AssetReference PrefabReference;
    }
}