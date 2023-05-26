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
        TurnRight,
        TurnLeft
    }

    [CreateAssetMenu(fileName = "LevelAssetsConfig", menuName = "Configs/LevelAssetsConfig")]
    public class LevelAssetsConfig : ScriptableObject
    {
        [SerializeField] private List<RoadPrefab> _roadPrefabs;

        public List<RoadPrefab> RoadPrefabs => _roadPrefabs;
    }

    [Serializable]
    public class RoadPrefab
    {
        public RoadBlockType Type;
        public AssetReference PrefabReference;
    }
}