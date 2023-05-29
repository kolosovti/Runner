using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Configs/MovementConfig")]
    public class MovementConfig : ScriptableObject
    {
        [SerializeField] private float _runnerSpeed;

        public float RunnerSpeed => _runnerSpeed;
    }
}