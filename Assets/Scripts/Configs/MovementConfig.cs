using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Configs/MovementConfig")]
    public class MovementConfig : ScriptableObject
    {
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _runnerSpeed;
        [SerializeField] private float _gravityForce;

        public float JumpForce => _jumpForce;
        public float RunnerSpeed => _runnerSpeed;
        public float GravityForce => _gravityForce;
    }
}