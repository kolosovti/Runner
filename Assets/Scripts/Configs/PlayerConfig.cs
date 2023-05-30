using UnityEngine;

namespace Game.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private Vector3 _jumpForce;
        [SerializeField] private int _maxJumpCount;
        [SerializeField] private float _maxHealth;
        
        public int MaxJumpCount => _maxJumpCount;
        public float MaxHealth => _maxHealth;
    }
}